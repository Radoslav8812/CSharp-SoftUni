
--3. Longest Magic Wand Per Deposit Groups
SELECT DepositGroup,
       --Count(*) AS Members,
       MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits
GROUP By DepositGroup

--4. * Smallest Deposit Group Per Magic Wand Size
SELECT TOP(2) 
       DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)

--5. Deposits Sum
SELECT
       DepositGroup,
       SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
GROUP BY DepositGroup

-- 6. Deposits Sum for Ollivander Family
SELECT
    DepositGroup,
    SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY SUM(DepositAmount) DESC

--9. Age Groups
SELECT AgeGroup,
       COUNT(*) AS WizzardsCount
FROM
(SELECT
    Age,
    CASE 
     WHEN Age BETWEEN 0 AND 10 Then '[0-10]'
     WHEN Age BETWEEN 11 AND 20 Then '[11-20]'
     WHEN Age BETWEEN 21 AND 30 Then '[21-30]'
     WHEN Age BETWEEN 31 AND 40 Then '[31-40]'
     WHEN Age BETWEEN 41 AND 50 Then '[41-50]'
     WHEN Age BETWEEN 51 AND 60 Then '[51-60]'
     WHEN Age >= 61 Then '[61+]'
     END AS AgeGroup
FROM WizzardDeposits) 
AS SubqueryAgeGroups
GROUP BY AgeGroup

--11. Average Interest
SELECT DepositGroup,
       IsDepositExpired,
       AVG(DepositInterest) AS AverageInterest
FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired

--12. * Rich Wizard, Poor Wizard
SELECT *
FROM WizzardDeposits AS wd1
JOIN WizzardDeposits AS wd2 ON wd1.Id + 1 = wd2.Id