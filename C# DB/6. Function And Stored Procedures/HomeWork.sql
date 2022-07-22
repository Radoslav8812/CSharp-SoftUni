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

--4.	Employees from Town
GO
CREATE OR ALTER PROCEDURE usp_GetEmployeesFromTown (@InputTown NVARCHAR(50))
AS
    SELECT FirstName,
           LastName
    FROM Employees AS e
    JOIN Addresses AS a ON e.AddressID = a.AddressID
    JOIN Towns AS t ON t.TownID = a.TownID
    WHERE t.Name = @InputTown

EXEC usp_GetEmployeesFromTown 'Monroe'
    