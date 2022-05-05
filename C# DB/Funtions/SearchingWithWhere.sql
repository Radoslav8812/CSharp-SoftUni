SELECT TOP (1000) [PersonID]
      ,[FirstName]
      ,[Salary]
      ,[PassportID]
  FROM [master].[dbo].[Persons]
  WHERE[FirstName] LIKE N'%Roberto%'