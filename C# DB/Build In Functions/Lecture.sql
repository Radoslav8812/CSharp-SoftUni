SELECT DISTINCT FirstName,
    LastName,
    JobTitle,
    CHARINDEX(N'Marketing', JobTitle) AS SearchedTitle
    FROM Employees
    WHERE JobTitle LIKE '%Specialist%'
    ORDER BY SearchedTitle DESC
    
SELECT FirstName + ' ' + MiddleName + ' ' + LastName AS [Full Name]
  FROM Employees
  WHERE (FirstName + ' ' + MiddleName + ' ' + LastName) IS NOT NULL
  ORDER BY [Full Name] ASC
  