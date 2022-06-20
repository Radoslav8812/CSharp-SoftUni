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

--3 Querying
--
SELECT
    id,
    FORMAT(JourneyStart, 'dd/MM/yyyy') AS JourneyStart,
    FORMAT(JourneyEnd, 'dd/MM/yyyy') AS JourneyEnd
 FROM Journeys
 WHERE Purpose IN ('Military')
 ORDER BY JourneyStart ASC
--
 SELECT c.Id,
        CONCAT(FirstName,' ', LastName) AS full_name
 FROM Colonists c
 Join TravelCards tc ON c.Id = tc.ColonistId
 WHERE JobDuringJourney IN ('Pilot')
 ORDER BY c.Id