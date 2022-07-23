--1.	Queries for SoftUni Database
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
    SELECT FirstName,
       LastName
    FROM Employees
    WHERE Salary > 35000

EXEC usp_GetEmployeesSalaryAbove35000

--2.	Employees with Salary Above Number
CREATE OR ALTER PROCEDURE usp_GetEmployeesSalaryAboveNumber (@InputSalary DECIMAL(18,4))
AS
    SELECT FirstName,
           LastName
    FROM Employees 
    WHERE Salary >= @InputSalary

EXEC usp_GetEmployeesSalaryAboveNumber 48100

--3.	Town Names Starting With
CREATE OR ALTER PROCEDURE usp_GetTownsStartingWith (@InputString NVARCHAR(50))
AS
    SELECT [Name]
    FROM Towns
    WHERE [Name] LIKE @InputString + '%'

EXEC usp_GetTownsStartingWith 's'
GO

--4.	Employees from Town
CREATE OR ALTER PROCEDURE usp_GetEmployeesFromTown (@InputTown NVARCHAR(50))
AS
BEGIN
    SELECT FirstName,
           LastName
    FROM Employees AS e
    JOIN Addresses AS a ON e.AddressID = a.AddressID
    JOIN Towns AS t ON t.TownID = a.TownID
    WHERE t.Name = @InputTown
END
EXEC usp_GetEmployeesFromTown 'Monroe'
GO

--5.	Salary Level Function
CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS VARCHAR(50)
AS
BEGIN
    DECLARE @SalaryLevel VARCHAR(50)

    IF @salary < 30000
    BEGIN
        SET @SalaryLevel = 'Low'
    END
    IF @salary BETWEEN 30000 AND 50000
    BEGIN
        SET @SalaryLevel = 'Average'
    END
    IF @salary > 50000
    BEGIN
        SET @SalaryLevel = 'High'
    END

    RETURN @SalaryLevel
END

GO
SELECT Salary,
       [dbo].[ufn_GetSalaryLevel](Salary) AS SalaryLevel
FROM Employees
GO

--6.	Employees by Salary Level
CREATE PROCEDURE usp_EmployeesBySalaryLevel @SalaryLevel VARCHAR(50)
AS
BEGIN
    SELECT FirstName,
           LastName
    FROM Employees
    WHERE [dbo].[ufn_GetSalaryLevel] (Salary) = @SalaryLevel
END

GO

EXEC [dbo].[usp_EmployeesBySalaryLevel] 'High'
EXEC [dbo].[usp_EmployeesBySalaryLevel] 'Average'
EXEC [dbo].[usp_EmployeesBySalaryLevel] 'Low'