--Write a SQL query to find first and last names of all employees whose first name starts with "SA". 
SELECT
    FirstName,
    LastName
FROM Employees
WHERE FirstName LIKE 'SA%'

--Find Names of All employees by Last Name
SELECT
    FirstName,
    LastName
FROM Employees
WHERE LastName LIKE '%EI%'

--Problem 3.	Find First Names of All Employees
SELECT
    FirstName
FROM Employees
WHERE DepartmentID IN (3, 10) AND YEAR(HireDate) BETWEEN 1995 AND 2005

-- Problem 4.	Find All Employees Except Engineers
SELECT
    FirstName,
    LastName
FROM Employees
WHERE JobTitle NOT LIKE '%engineer%'

--Problem 5.	Find Towns with Name Length
SELECT [Name]
FROM Towns
WHERE LEN([Name]) IN (5, 6)
ORDER BY [Name] ASC

--Problem 6.	 Find Towns Starting With
SELECT [TownID],
    [Name]
FROM Towns
WHERE SUBSTRING([Name], 1, 1) NOT IN ('M', 'K', 'B', 'E')
ORDER BY [Name] ASC

--Problem 7.	 Find Towns Not Starting With
SELECT [TownID],
    [Name]
FROM Towns
WHERE SUBSTRING([Name], 1, 1) NOT IN ('R', 'B', 'D')
ORDER BY [Name] ASC

--Problem 8.	Create View Employees Hired After 2000 Year
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT
    FirstName,
    LastName
FROM Employees
WHERE YEAR(HireDate) > 2000

--Problem 9.	Length of Last Name
SELECT
    FirstName,
    LastName
FROM Employees
WHERE LEN(LastName) = 5

--Problem 10.	Rank Employees by Salary
SELECT
    EmployeeID,
    FirstName,
    LastName,
    Salary,
    DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeID) AS [Rank]
FROM Employees
    WHERE Salary BETWEEN 10000 AND 50000
    ORDER BY Salary DESC

-- Problem 11.	Find All Employees with Rank 2 *
SELECT * FROM
(SELECT EmployeeID,
        FirstName,
        LastName,
        Salary,
        DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeID) AS [Rank]
        FROM Employees
        WHERE Salary BETWEEN 10000 AND 50000)
        AS ResultTable
WHERE [Rank] = 2
ORDER BY Salary DESC

--Problem 12.	Countries Holding ‘A’ 3 or More Times
SELECT CountryName, IsoCode
FROM Countries
WHERE CountryName LIKE '%A%A%A%'
ORDER BY IsoCode ASC

--Problem 13.	 Mix of Peak and River Names
SELECT PeakName,
    RiverName,
    LOWER(LEFT(PeakName, LEN(PeakName) - 1) + RIGHT(RiverName, LEN(RiverName))) AS Mix
FROM Peaks, Rivers
WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
ORDER BY Mix ASC