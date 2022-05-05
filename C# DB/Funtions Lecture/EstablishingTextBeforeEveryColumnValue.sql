SELECT TOP (1000) [PersonID]
      ,[FirstName], STUFF ([FirstName], 1, 0, 'SoftUni Unique Name: ')
      ,[Salary]
      ,[PassportID]
  FROM [master].[dbo].[Persons]