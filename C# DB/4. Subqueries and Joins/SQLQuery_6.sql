
-- 1.	Employee Address
SELECT TOP(5) e.EmployeeID, e.JobTitle, a.AddressID, a.AddressText
FROM Employees AS [e]
JOIN Addresses AS [a] ON [e].AddressID = [a].[AddressID]
ORDER BY AddressID

--2.	Addresses with Towns
SELECT TOP(50) e.FirstName, e.LastName, t.Name, a.AddressText
FROM Employees AS [e]
JOIN Addresses AS [a] ON [e].AddressID = [a].[AddressID]
JOIN Towns as [t] ON [t].TownID = [a].TownID
ORDER BY FirstName ASC, LastName ASC

--3.	Sales Employee
SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name
FROM Employees AS [e]
JOIN Departments as [d] ON [e].DepartmentID = [d].DepartmentID
WHERE [d].Name IN ('Sales')
ORDER BY EmployeeID ASC

--4.	Employee Departments
SELECT TOP(5) e.EmployeeID, e.FirstName, e.Salary, d.Name
FROM Employees AS [e]
JOIN Departments AS [d] ON [e].DepartmentID = [d].DepartmentID
WHERE [e].Salary > 15000
ORDER BY [d].DepartmentID ASC

--5.	Employees Without Project
SELECT TOP(3) e.EmployeeID, e.FirstName
FROM Employees AS [e]
LEFT JOIN EmployeesProjects AS [ep] ON [e].EmployeeID = [ep].EmployeeID
WHERE [ep].EmployeeID IS NULL
ORDER BY [e].EmployeeID ASC

--6. Employees Hired After
SELECT [e].[FirstName], [e].[LastName], [e].[HireDate], [d].[Name]
FROM [Employees] AS [e]
JOIN [Departments] AS [d] ON [e].[DepartmentID] = [d].[DepartmentID]
WHERE HireDate > '1.1.1999'AND [d].[Name] IN ('Sales', 'Finance')
ORDER BY [e].[HireDate] ASC

--7.	Employees with Project
SELECT e.EmployeeID, e.FirstName, p.Name AS ProjectName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep On e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p ON ep.ProjectID = p.ProjectID ???