SELECT *, DATEPART(WEEKDAY, HireDate) AS [CuttedWeekDay],
        DATEPART(DAYOFYEAR, HireDate) AS [CuttedDay],
        DATEPART(QUARTER, HireDate) AS [Quarter]
    FROM Employees
    ORDER BY [Quarter] DESC
--
-- DATEPART
SELECT FirstName + ' ' + LastName ,
    HireDate,
    DATEPART(QUARTER, HireDate) AS [Quarter],
    DATEPART(MONTH, HireDate) AS [Month],
    DATEPART(DAY, HireDate) AS [Day]
FROM Employees
--
-- DATEDIFF
SET LANGUAGE English
SELECT FirstName + ' ' + LastName ,
    HireDate,
    DATEDIFF(DAY, HireDate, GETDATE()) AS [DiapasonDays],
    DATEDIFF(MONTH, HireDate, GETDATE()) AS [DiapasonMonths],
    DATEDIFF(YEAR, HireDate, GETDATE()) AS [DiapasonYears],
    DATENAME(WEEKDAY, HireDate) AS [DayName]
FROM Employees
ORDER BY DiapasonYears DESC
--
-- ACROSS PAGES
SELECT * FROM Employees
ORDER BY HireDate
OFFSET (10- 1) * 10 ROWS
FETCH NEXT 10 ROWS ONLY
--
-- RANKS
SELECT
    Salary,
    ROW_NUMBER() OVER (ORDER BY Salary DESC) AS RowNumber,
    RANK() OVER (ORDER BY Salary DESC) AS [Rank],
    DENSE_RANK() OVER (ORDER BY Salary DESC) AS [DenseRank],
    NTILE(100) OVER (ORDER BY Salary DESC) AS [NTile]
    FROM Employees