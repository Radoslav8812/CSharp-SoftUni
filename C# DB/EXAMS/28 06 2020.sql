USE ColonialJurney

CREATE DATABASE ColonialJourney

--1 Design
CREATE TABLE Planets(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(30) NOT NULL
)
CREATE TABLE Spaceports(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    PlanetId INT REFERENCES Planets(Id) NOT NULL
)
CREATE TABLE Spaceships(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    Manufacturer VARCHAR(30) NOT NULL,
    LightSpeedRate INT DEFAULT 0
)
CREATE TABLE Colonists(
    Id INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(20) NOT NULL,
    LastName VARCHAR(20) NOT NULL,
    Ucn VARCHAR(10) NOT NULL UNIQUE,
    BirthDate DATE NOT NULL
)
CREATE TABLE Journeys(
    Id INT PRIMARY KEY IDENTITY,
    JourneyStart DATETIME2 NOT NULL,
    JourneyEnd DATETIME2 NOT NULL,
    Purpose VARCHAR(11) CHECK (Purpose IN('Medical', 'Technical', 'Educational', 'Military')),
    DestinationSpaceportId INT REFERENCES Spaceports(Id) NOT NULL,
    SpaceshipId INT REFERENCES Spaceships(Id) NOT NULL
)
CREATE TABLE TravelCards(
    Id Int PRIMARY KEY IDENTITY,
    CardNumber CHAR(10) NOT NULL UNIQUE,
    JobDuringJourney VARCHAR(8) CHECK (JobDuringJourney IN('Pilot', 'Engineer', 'Trooper', 'Cleaner', 'Cook')),
    ColonistId INT REFERENCES Colonists(Id) NOT NULL,
    JourneyId INT REFERENCES Journeys(Id) NOT NULL
)

--Section 2. DML
INSERT INTO Planets VALUEs
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships VALUES
('Golf', 'VW', 3),
('WakaWaka', 'Wakanda', 4),
('Falcon9', 'SpaceX', 1),
('Bed', 'Vidolov', 6)

--3.	Update
--Update all spaceships light speed rate with 1 where the Id is between 8 and 12.
UPDATE Spaceships
SET LightSpeedRate += 1
WHERE Id BETWEEN 8 AND 12

--4.	Delete
--Delete first three inserted Journeys (be careful with the relationships).
DELETE
FROM TravelCards
WHERE JourneyId BETWEEN 1 AND 3

DELETE
FROM Journeys
WHERE Id BETWEEN 1 AND 3

--3 Querying
--5.	Select all military journeys
--Extract from the database, all Military journeys in the format "dd-MM-yyyy". Sort the results ascending by journey start.
SELECT
    id,
    FORMAT(JourneyStart, 'dd/MM/yyyy') AS JourneyStart,
    FORMAT(JourneyEnd, 'dd/MM/yyyy') AS JourneyEnd
 FROM Journeys
 WHERE Purpose IN ('Military')
 ORDER BY JourneyStart ASC


--6.	Select all pilots
--Extract from the database all colonists, which have a pilot job. Sort the result by id, ascending.

 SELECT 
        c.Id,
        CONCAT(FirstName,' ', LastName) AS full_name
 FROM Colonists c
 JOIN TravelCards tc ON c.Id = tc.ColonistId
 WHERE JobDuringJourney IN ('Pilot')
 ORDER BY c.Id

 --7.	Count colonists
--Count all colonists that are on technical journey. 
SELECT 
    COUNT(*) AS [count]
FROM Colonists AS c
JOIN TravelCards AS tc ON c.Id = tc.ColonistId
JOIN Journeys AS j ON tc.JourneyId = j.Id
WHERE j.Purpose IN ('Technical')

--8.	Select spaceships with pilots younger than 30 years
--Extract from the database those spaceships, which have pilots, younger than 30 years old. 
--In other words, 30 years from 01/01/2019. Sort the results alphabetically by spaceship name.
SELECT
    s.Name,
    s.Manufacturer
FROM Spaceships AS s
JOIN Journeys AS j ON s.id = j.SpaceshipId
JOIN TravelCards AS tc ON j.Id = tc.JourneyId
JOIN Colonists AS c ON tc.ColonistId = c.Id
WHERE c.BirthDate > '1989/01/01' AND c.BirthDate < '2019/01/01' AND tc.JobDuringJourney = 'Pilot' 
ORDER BY s.[Name]

--9.	Select all planets and their journey count
--Extract from the database all planets’ names and their journeys count. 
--Order the results by journeys count, descending and by planet name ascending.

SELECT 
    p.Name,
    COUNT(j.Id) AS JourneyCount
FROM Planets AS p
JOIN Spaceports AS sp ON p.Id = sp.PlanetId
JOIN Journeys AS j ON sp.Id = j.DestinationSpaceportId
GROUP BY p.Name
ORDER BY JourneyCount DESC, p.Name ASC

--10.	Select Second Oldest Important Colonist
--Find all colonists and their job during journey with rank 2. 
--Keep in mind that all the selected colonists with rank 2 must be the oldest ones.
--You can use ranking over their job during their journey.
SELECT 
    * 
FROM
    (SELECT 
    tc.JobDuringJourney,
    CONCAT(FirstName, ' ', LastName) AS [FullName],
    DENSE_RANK() OVER(PARTITION BY tc.JobDuringJourney ORDER BY c.BirthDate) AS [JobRank]
FROM Colonists AS c
JOIN TravelCards AS tc ON c.Id = tc.ColonistId) AS temp
WHERE temp.JobRank = 2

GO
--11.	Get Colonists Count
--Create a user defined function with the name dbo.udf_GetColonistsCount(PlanetName VARCHAR (30)) 
--that receives planet name and returns the count of all colonists sent to that planet.
CREATE FUNCTION udf_GetColonistsCount(@PlanetName VARCHAR(30))
RETURNS INT
AS
BEGIN
    RETURN
    (SELECT
        COUNT(c.Id)
    FROM Planets AS p
    JOIN Spaceports AS sp ON p.Id = sp.PlanetId
    JOIN Journeys AS j ON sp.Id = j.DestinationSpaceportId
    JOIN TravelCards AS tc ON j.Id = tc.JourneyId
    JOIN Colonists AS c ON tc.ColonistId = c.Id
    WHERE p.Name = @PlanetName)
END

SELECT dbo.udf_GetColonistsCount('Otroyphus')
GO

--12 Create a user defined stored procedure, named usp_ChangeJourneyPurpose(@JourneyId, @NewPurpose), that receives an journey id and purpose, and attempts to change the purpose of that journey.
-- An purpose will only be changed if all of these conditions pass:
--•	If the journey id doesn’t exists, then it cannot be changed. Raise an error with the message “The journey does not exist!”
--•	If the journey has already that purpose, raise an error with the message “You cannot change the purpose!”

CREATE OR ALTER PROC usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(11))
AS
BEGIN

    DECLARE @PurposeToChange VARCHAR(MAX) =
    (SELECT
        Purpose
    FROM Journeys
    WHERE Id = @JourneyId)

    IF @JourneyId NOT IN (SELECT Id FROM Journeys)
    BEGIN
        RAISERROR('The journey does not exist!', 16, 1)
    END

    IF @NewPurpose = (SELECT Purpose FROM Journeys WHERE Id = @JourneyId)
    BEGIN
        RAISERROR('You cannot change the purpose!', 16, 2)
    END 

    UPDATE Journeys
    SET Purpose = @NewPurpose
    WHERE Id = @JourneyId

END

EXEC usp_ChangeJourneyPurpose 4, 'Technical'
EXEC usp_ChangeJourneyPurpose 2, 'Educational'
EXEC usp_ChangeJourneyPurpose 196, 'Technical'
