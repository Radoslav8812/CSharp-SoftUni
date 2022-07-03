SELECT TOP(50)
    e.EmployeeID,
    CONCAT(e.FirstName, ' ',e.LastName) AS EmployeeName,
    CONCAT(m.FirstName, ' ',m.LastName) AS ManagerName
FROM Employees AS e
LEFT JOIN Employees AS m ON e.ManagerID = m.EmployeeID
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID