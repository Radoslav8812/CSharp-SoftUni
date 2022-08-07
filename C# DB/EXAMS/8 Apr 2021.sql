CREATE DATABASE [Service]
GO

--Section 1. DDL (30 pts)
CREATE TABLE Users
(
    Id INT PRIMARY KEY IDENTITY, 
    Username VARCHAR(30) UNIQUE NOT NULL,
    [Password] VARCHAR(50) NOT NULL,
    [Name] VARCHAR(50),
    Birthdate DATETIME,
    Age INT CHECK (Age >= 14 AND Age <= 110),
    Email VARCHAR(50) NOT NULL
)

CREATE TABLE Departments
(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Employees
(
    Id INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(25),
    LastName VARCHAR(25),
    Birthdate DATETIME,
    Age INT CHECK (Age >= 18 AND Age <= 110),
    DepartmentId INT REFERENCES Departments(Id)
)

CREATE TABLE Categories
(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    DepartmentId INT REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE [Status]
(
    Id INT PRIMARY KEY IDENTITY,
    Label VARCHAR(30) NOT NULL
)

CREATE TABLE Reports
(
    Id INT PRIMARY KEY IDENTITY,
    CategoryId INT REFERENCES Categories(Id),
    StatusId INT REFERENCES [Status](Id),
    OpenDate DATETIME NOT NULL,
    CloseDate DATETIME,
    [Description] VARCHAR(200) NOT NULL,
    UserId INT REFERENCES Users(Id) NOT NULL,
    EmployeeId INT REFERENCES Employees(Id)
)

--2.	Insert
INSERT INTO Employees (FirstName, LastName, Birthdate, DepartmentId) VALUES
('Marlo', 'O''Malley', '1958-9-21', 1),
('Niki', 'Stanaghan', '1969-11-26', 4),
('Ayrton', 'Senna',	'1960-03-21', 9),
('Ronnie', 'Peterson', '1944-02-14', 9),
('Giovanna', 'Amati', '1959-07-20', 5)

INSERT INTO Reports VALUES
(1,	1, '2017-04-13', NULL, 'Stuck Road on Str.133', 6, 2),
(6,	3, '2015-09-05', 2015-12-06,'Charity trail running' ,3, 5),
(14, 2,	'2015-09-07', NULL ,'Falling bricks on Str.58',	5,	2),
(4,	3,	'2017-07-03', 2017-07-06, 'Cut off streetlight on Str.11',1, 1)


--3 Update
--Update the CloseDate with the current date of all reports, which don't have CloseDate. 

UPDATE Reports
SET CloseDate = GETDATE()
WHERE CloseDate IS NULL

--
DELETE
FROM Reports
WHERE StatusId = 4


--Section 3. Querying (40 pts)
--5.	Unassigned Reports
--Find all reports that don't have an assigned employee. Order the results by OpenDate in ascending order, 
--then by description ascending. OpenDate must be in format - 'dd-MM-yyyy'
SELECT
    [Description],
    FORMAT(OpenDate, 'dd-MM-yyyy') AS [Open Date]
FROM Reports
WHERE EmployeeId IS NULL
ORDER BY [OpenDate] ASC, [Description] ASC

--6.	Reports & Categories
--Select all descriptions from reports, which have category. Order them by description (ascending) then by category name (ascending).
SELECT
    [Description],
    c.Name AS CategoryName
FROM Reports AS r
JOIN Categories AS c ON c.Id = r.CategoryId
ORDER BY [Description] ASC, c.Name ASC

--7.	Most Reported Category
--Select the top 5 most reported categories and order them by the number of reports per category in descending order and then alphabetically by name.
SELECT TOP (5)
    c.Name AS CategoryName,
    COUNT(CategoryId) AS ReportsNumber
FROM Categories AS c
JOIN Reports AS r ON r.CategoryId = c.Id
GROUP BY c.Name
ORDER BY ReportsNumber DESC , c.Name ASC

--8.	Birthday Report
-- Select the user's username and category name in all reports in which users have submitted a report on their birthday.
-- Order them by username (ascending) and then by category name (ascending).

SELECT 
    u.Username,
    c.Name AS CategoryName
FROM Users AS u
JOIN Reports AS r ON u.Id = r.UserId
JOIN Categories AS c On r.CategoryId = c.Id
WHERE MONTH(u.Birthdate) = MONTH(r.OpenDate) AND DAY(u.Birthdate) = DAY(r.OpenDate)
ORDER BY u.Username ASC, c.[Name] ASC

--9.	Users per Employee 
--Select all employees and show how many unique users each of them has served to.
--Order by users count  (descending) and then by full name (ascending).
SELECT 
    CONCAT(e.FirstName, ' ', e.LastName) AS FullName,
    COUNT(u.Id) AS UserCount
FROM Employees AS e
LEFT JOIN Reports AS r ON e.Id = r.EmployeeId
LEFT JOIN Users AS u ON r.UserId = u.Id
GROUP BY CONCAT(e.FirstName, ' ', e.LastName)
ORDER BY UserCount DESC, FullName ASC

SELECT * FROM Reports
SELECT * FROM Categories

--10

SELECT
    ISNULL(e.FirstName + ' ' + e.LastName, 'None') AS Employee,
    ISNULL(d.Name, 'None'),
    ISNULL(c.Name, 'None') AS Category,
    r.[Description],
    FORMAT(r.OpenDate, 'dd.MM.yyyy') AS OpenDate,
    s.Label AS [Status],
    ISNULL(u.Name, 'None') AS [User]
FROM Reports AS [r]
LEFT JOIN Employees AS e ON e.Id = r.EmployeeId
LEFT JOIN Users AS u ON u.Id = r.UserId
LEFT JOIN Categories AS c ON c.Id = r.CategoryId
LEFT JOIN Departments AS d ON d.Id = e.DepartmentId
LEFT JOIN [Status] AS s ON s.Id = r.StatusId
ORDER BY FirstName DESC, LastName DESC, d.Name ASC, c.Name ASC, r.[Description] ASC, r.OpenDate ASC, s.Label ASC, u.Username ASC

--11.	Hours to Complete
--Create a user defined function with the name udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME) that receives a start date and end date and 
--must returns the total hours which has been taken for this task. If start date is null or end is null return 0.

GO

CREATE OR ALTER FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN
    
    IF (@StartDate IS NULL)
        RETURN 0;
    IF (@EndDate IS NULL)
        RETURN 0;

    RETURN DATEDIFF(HOUR, @StartDate, @EndDate)
END

SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports

GO

CREATE OR ALTER PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
    
    DECLARE @EmployeeDepartmentId INT = 
    (SELECT DepartmentId FROM Employees
    WHERE id = @EmployeeId)

    DECLARE @ReportDepartmentId INT =
    (SELECT c.DepartmentId FROM Reports AS r
    JOIN Categories AS c ON c.Id = r.CategoryId
    WHERE r.Id = @ReportId)

    IF(@EmployeeDepartmentId != @ReportDepartmentId)
    THROW 50000, 'Employee doesn''t belong to the appropriate department!', 1

    UPDATE Reports SET EmployeeId = @EmployeeId
    WHERE Id = @ReportId
END

EXEC usp_AssignEmployeeToReport 30, 1
EXEC usp_AssignEmployeeToReport 17, 2