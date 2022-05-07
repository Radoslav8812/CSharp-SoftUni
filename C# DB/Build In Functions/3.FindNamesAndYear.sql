--Write a SQL query to find the first names of all employees in the departments with ID 3 or 10 and whose hire year is between 1995 and 2005 inclusive.

SELECT FirstName FROM Employees WHERE DepartmentID = 3 OR DepartmentID = 10 AND DATEPART(year, HireDate) >= 1995 AND DATEPART(year, HireDate) <= 2005