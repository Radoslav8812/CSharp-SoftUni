--Write a SQL query to find first and last names of all employees whose last name contains "ei". 
SELECT FirstName, LastName FROM Employees WHERE LastName LIKE '%ei%'