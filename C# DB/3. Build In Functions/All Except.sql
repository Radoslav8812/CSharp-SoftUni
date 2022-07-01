--Write a SQL query to find the first and last names of all employees whose job titles does not contain "engineer". 
SELECT FirstName, LastName FROM Employees WHERE JobTitle  NOT LIKE '%engineer'
