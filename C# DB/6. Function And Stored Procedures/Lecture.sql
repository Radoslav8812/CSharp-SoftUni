ALTER FUNCTION udf_GetSalaryLevel(@Salary MONEY)
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @Result VARCHAR(10)
    Set @Result = 'High'
    IF (@Salary < 30000)
    BEGIN
        SET @Result = 'Low'
    END
    ELSE IF(@Salary BETWEEN 30000 AND 50000)
    BEGIN
        SET @Result = 'Average'
    END
    RETURN @Result
END

SELECT
    CONCAT(FirstName, ' ', LastName) AS [Full Name],
    Salary,
    dbo.udf_GetSalaryLevel(Salary) AS [Salary Level]
FROM Employees