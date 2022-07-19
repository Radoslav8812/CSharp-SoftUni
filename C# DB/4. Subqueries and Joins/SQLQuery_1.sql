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
SELECT TOP(5) e.EmployeeID, e.FirstName, p.Name AS ProjectName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep On e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p ON ep.ProjectID = p.ProjectID
WHERE (p.StartDate > '2002-08-13' AND p.EndDate IS NULL)
ORDER BY e.EmployeeID

--8.	Employee 24
SELECT e.EmployeeID, e.FirstName, ProjectName =
CASE
WHEN DATEPART(YEAR, p.StartDate) >= 2005
THEN NULL
ELSE p.Name
END
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep On ep.EmployeeID = e.EmployeeID
LEFT JOIN Projects AS p ON ep.ProjectID = p.ProjectID
WHERE e.EmployeeID = 24

--9.	Employee Manager
SELECT e.EmployeeID, e.FirstName, e.ManagerID, emp.FirstName AS ManagerName
FROM Employees AS e
JOIN Employees AS emp On e.ManagerID = emp.EmployeeID
WHERE e.ManagerID IN (3, 7)
ORDER BY e.EmployeeID ASC

--10. Employee Summary
SELECT TOP(50) e.EmployeeID, e.FirstName + ' ' + e.LastName AS EmployeeName, emp.FirstName + ' ' + emp.LastName AS ManagerName, d.Name AS DepartmentName
FROM Employees AS e
JOIN Employees AS emp ON e.ManagerID = emp.EmployeeID
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID ASC

--11. Min Average Salary
SELECT TOP(1) AVG(Salary) AS MinAverageSalary
FROM Employees
GROUP BY DepartmentID
ORDER BY MinAverageSalary ASC

--12. Highest Peaks in Bulgaria
SELECT c.CountryCode, m.MountainRange, p.PeakName, p.Elevation
FROM MountainsCountries AS mc
JOIN Countries AS c ON mc.CountryCode = c.CountryCode
JOIN Mountains AS m ON mc.MountainId = m.Id
JOIN Peaks AS p ON p.MountainId = m.Id
WHERE p.Elevation > 2835 AND c.CountryCode = 'BG'
ORDER BY p.[Elevation] DESC

--13. Count Mountain Ranges
SELECT mc.CountryCode, COUNT(CountryCode) AS MountainRanges
FROM MountainsCountries AS mc
WHERE CountryCode IN ('BG', 'US', 'RU') 
GROUP BY [CountryCode]

--14. Countries with Rivers
SELECT TOP(5) c.CountryName, r.RiverName 
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName ASC

--15. *Continents and Currencies
SELECT
    ContinentCode,
    CurrencyCode,
    CurrencyUsage
FROM(
    SELECT 
        ContinentCode,
        CurrencyCode,
        DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY COUNT(*) DESC) AS Ranking,
        COUNT(*) AS CurrencyUsage
    FROM Countries
    GROUP BY ContinentCode, CurrencyCode) AS RankedTable
WHERE Ranking = 1 AND CurrencyUsage > 1
ORDER BY ContinentCode, CurrencyCode

--16. Countries without mountains
SELECT COUNT(*) AS [COUNT]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
WHERE mc.CountryCode IS NULL


--17. Highest Peak Name and Elevation by Country
SELECT TOP(5)
    c.CountryName,
    MAX(p.Elevation) AS HighestPeakElevation,
    MAX(r.Length) AS LongestRiverLength
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = m.Id
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, CountryName

--18. Highest Peak Name and Elevation by Country
SELECT TOP(5)
    Country,
CASE
WHEN PeakName IS NULL THEN '(no highest peak)'
ELSE PeakName
END AS [Highest Peak Name],
CASE
WHEN Elevation IS NULL THEN 0
ELSE Elevation
END AS [Highest Peak Elevation],
CASE
WHEN MountainRange IS NULL THEN '(no mountain)'
ELSE MountainRange
END AS [Mountain]
FROM(
    SELECT 
        c.CountryName AS Country,
        m.MountainRange AS MountainRange,
        p.PeakName,
        p.Elevation,
        DENSE_RANK() OVER(PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS PeakRank
    FROM Countries AS c
    LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
    LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
    LEFT JOIN Peaks AS p ON p.MountainId = m.Id
    ) AS PeakRank
WHERE PeakRank = 1
ORDER BY Country ASC, [Highest Peak Name] ASC