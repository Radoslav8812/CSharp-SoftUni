CREATE DATABASE Zoo
GO


--Section 1. DDL (30 pts)
CREATE TABLE Owners(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    [Address] VARCHAR(50)
)

CREATE TABLE AnimalTypes(
    Id INT PRIMARY KEY IDENTITY,
    AnimalType VARCHAR(30) NOT NULL
)

CREATE TABLE Cages(
    Id INT PRIMARY KEY IDENTITY,
    AnimalTypeId INT REFERENCES AnimalTypes([Id]) NOT NULL
)

CREATE TABLE Animals(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(30) NOT NULL,
    BirthDate DATE NOT NULL,
    OwnerId INT REFERENCES Owners([Id]),
    AnimalTypeId INT REFERENCES AnimalTypes([Id]) NOT NULL
)

CREATE TABLE AnimalsCages(
    CageId INT REFERENCES Cages([Id]) NOT NULL,
    AnimalId INT REFERENCES Animals([Id]) NOT NULL,

    PRIMARY KEY(CageId, AnimalId)
)

CREATE TABLE VolunteersDepartments(
    Id INT PRIMARY KEY IDENTITY,
    DepartmentName VARCHAR(30) NOT NULL
)

CREATE TABLE Volunteers(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    [Address] VARCHAR(50),
    AnimalId INT REFERENCES Animals([Id]),
    DepartmentId INT REFERENCES VolunteersDepartments([ID]) NOT NULL
)


--Section 2. DML (10 pts)

INSERT INTO Volunteers VALUES
('Anita Kostova', '0896365412', 'Sofia, 5 Rosa str.', 15, 1),
('Dimitur Stoev', '0877564223', null, 42, 4),
('Kalina Evtimova',	'0896321112', 'Silistra, 21 Breza str.', 9,	7),
('Stoyan Tomov', '0898564100', 'Montana, 1 Bor str.', 18, 8),
('Boryana Mileva', '0888112233', null, 31 ,5)

INSERT INTO Animals VALUES
('Giraffe', '2018-09-21', 21, 1),
('Harpy Eagle', '2015-04-17', 15, 3),
('Hamadryas Baboon', '2017-11-02', null, 1),
('Tuatara', '2021-06-30', 2, 4)

--3.	Update
UPDATE Animals
SET OwnerId = 4
WHERE OwnerId IS NULL

--4
DELETE
FROM Volunteers
WHERE DepartmentId = 2

DELETE
FROM VolunteersDepartments
WHERE [DepartmentName] IN ('Education program assistant')

--Section 3. Querying (40 pts)
--5.	Volunteers
SELECT
    [Name],
    PhoneNumber,
    [Address],
    [AnimalId],
    [DepartmentId]
FROM Volunteers
ORDER BY [Name] ASC, AnimalId ASC, DepartmentId ASC

--6.	Animals data
SELECT
    a.Name,
    at.AnimalType,
    FORMAT(BirthDate,'dd.MM.yyyy') AS [BirthDate]
FROM Animals AS a
JOIN AnimalTypes AS [at] ON [at].[Id] = a.AnimalTypeId
ORDER BY a.Name ASC

--7.	Owners and Their Animals
SELECT TOP(5)
    o.[Name],
    COUNT(a.Id) AS CountOfAnimals
FROM Owners AS o
JOIN Animals AS a ON a.OwnerId = o.Id
GROUP BY o.Name
ORDER BY CountOfAnimals DESC, o.Name ASC

--8.	Owners, Animals and Cages
SELECT
    CONCAT(o.[Name], '-', a.[Name]) AS [OwnersAnimals],
    o.PhoneNumber,
    ac.CageId
FROM Owners AS o
JOIN Animals AS a ON a.OwnerId = o.Id
JOIN AnimalsCages AS ac ON ac.AnimalId = a.Id
WHERE a.AnimalTypeId = 1
ORDER BY o.[Name] ASC, a.[Name] DESC

--T09 Volunteers in Sofia
SELECT
    v.Name,
    v.PhoneNumber,
    SUBSTRING([Address], CHARINDEX(',', [Address], 1 ) + 1, LEN([Address])) AS [Address]
FROM Volunteers AS v
JOIN VolunteersDepartments AS vd ON vd.Id = v.DepartmentId
WHERE vd.Id = 2 AND v.Address LIKE '%Sofia%'
ORDER BY v.Name ASC


--10.	Animals for Adoption
SELECT
    a.[Name],
    YEAR(a.BirthDate) AS BirthYear,
    [at].AnimalType
FROM Animals AS [a]
JOIN AnimalTypes AS [at] On [at].Id = a.AnimalTypeId
WHERE OwnerId IS NULL AND DATEDIFF(YEAR, a.BirthDate, '01/01/2022') < 5 AND [at].AnimalType != 'Birds'
ORDER BY a.Name

GO

--11.	All Volunteers in a Department


CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(30))
RETURNS INT
AS
BEGIN
		DECLARE @count INT = 
        (
        SELECT COUNT(*) 
		FROM Volunteers 
		WHERE DepartmentId = 
            (
            SELECT Id FROM VolunteersDepartments
            WHERE DepartmentName = @VolunteersDepartment
            )
        )
	RETURN @count;
END

SELECT dbo.udf_GetVolunteersCountFromADepartment ('Education program assistant')
SELECT dbo.udf_GetVolunteersCountFromADepartment ('Guest engagement')

GO

--12
CREATE OR ALTER PROCEDURE  usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(30))
AS
BEGIN

    DECLARE @ownerId INT =
    (
        SELECT OwnerId
        FROM Animals
        WHERE [Name] = @AnimalName
    )

    IF (@ownerId IS NULL)
        BEGIN
        SELECT
        [Name],
        'For adoption' AS OwnerName
        FROM Animals
        WHERE [Name] = @AnimalName
        END
    ELSE
        BEGIN
        SELECT
        a.[Name],
        o.[Name] AS OwnersName
        FROM Animals AS [a]
        JOIN Owners AS o ON a.OwnerId = o.Id
        WHERE a.[Name] = @AnimalName
    END
END

 EXEC usp_AnimalsWithOwnersOrNot 'Hippo'
 EXEC usp_AnimalsWithOwnersOrNot 'Pumpkinseed Sunfish'