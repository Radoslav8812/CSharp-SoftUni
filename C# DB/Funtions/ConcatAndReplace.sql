--Concat with Replace from 2 whitespaces to 1.
SELECT REPLACE(CONCAT(PersonID, ' ', FirstName, ' ', Salary), '  ', ' ') As Concatenated
FROM [master].[dbo].[Persons]