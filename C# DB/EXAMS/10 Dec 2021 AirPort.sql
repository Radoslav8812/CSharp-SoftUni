CReate DATABASE Airport
GO

CREATE TABLE Passengers
(
    Id INT PRIMARY KEY IDENTITY,
    FullName VARCHAR(100) UNIQUE NOT NULL,
    Email VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Pilots
(
    Id INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(30) UNIQUE NOT NULL,
    LastName VARCHAR(30) UNIQUE NOT NULL,
    Age TINYINT NOT NULL CHECK (Age >= 21 AND Age <= 62),
    Rating FLOAT (53) CHECK (Rating >= 0.0 AND Rating <= 10.0)
)

CREATE TABLE AircraftTypes
(
    Id INT PRIMARY KEY IDENTITY,
    TypeName VARCHAR(30) UNIQUE NOT NULL
)

CREATE TABLE Aircraft
(
    Id INT PRIMARY KEY IDENTITY,
    Manufacturer VARCHAR(25) NOT NULL,
    Model VARCHAR(30) NOT NULL,
    [Year] INT NOT NULL,
    FlightHours INT,
    Condition CHAR(1) NOT NULL,
    TypeId INT REFERENCES AircraftTypes(Id) NOT NULL
)

CREATE TABLE PilotsAircraft
(
    AircraftId INT REFERENCES Aircraft(Id) NOT NULL,
    PilotId INT REFERENCES Pilots(Id) NOT NULL,

    PRIMARY KEY (AircraftId, PilotId)
)

CREATE TABLE Airports
(
    Id INT PRIMARY KEY IDENTITY,
    AirportName VARCHAR(70) UNIQUE NOT NULL,
    Country VARCHAR(100) UNIQUE NOT NULL
)

CREATE TABLE FlightDestinations
(
    Id INT PRIMARY KEY IDENTITY,
    AirportId INT REFERENCES Airports(Id) NOT NULL,
    [Start] DATETIME NOT NULL,
    AircraftId INT REFERENCES Aircraft(Id) NOT NULL,
    PassengerId INT REFERENCES Passengers(Id) NOT NULL,
    TicketPrice DECIMAL(18,2) DEFAULT 15 NOT NULL
)


--Section 2. DML (10 pts)
--2.	Insert
DECLARE @count INT = 5;

WHILE (@count <= 15)
BEGIN
    INSERT INTO Passengers VALUES
    ((SELECT CONCAT(FirstName, ' ', LastName) FROM Pilots WHERE Id = @count),
    (SELECT (CONCAT(FirstName, LastName, '@gmail.com')) FROM Pilots WHERE Id = @count))
SET @count += 1
END

--3.	Update
--Update all Aircraft, which:
-- Have a condition of 'C' or 'B' and
-- Have FlightHours Null or up to 100 (inclusive) and
-- Have Year after 2013 (inclusive)
-- By setting their condition to 'A'.
UPDATE Aircraft
SET Condition = 'A'
WHERE Condition IN ('C', 'B') AND (FlightHours IS NULL OR FlightHours <= 100) AND ([Year] > 2013)

--4.	Delete
--Delete every passenger whose FullName is up to 10 characters (inclusive) long.
DELETE 
FROM Passengers
WHERE LEN(FullName) <= 10


--Section 3. Querying (40 pts)
--5 Extract information about all the Aircraft. Order the results by aircraft’s FlightHours descending.
SELECT
    Manufacturer,
    Model,
    FlightHours,
    Condition
FROM Aircraft
ORDER BY FlightHours DESC

--6.	Pilots and Aircraft
--Select pilots and aircraft that they operate. Extract the pilot’s First, Last names, aircraft’s Manufacturer, Model, 
--and FlightHours. Skip all plains with NULLs and up to 304 FlightHours. Order the result by the FlightHours in descending order, then by the pilot’s FirstName alphabetically.
SELECT 
    p.FirstName,
    p.LastName,
    a.Manufacturer,
    a.Model,
    a.FlightHours
FROM PilotsAircraft AS pa
JOIN Aircraft AS a ON a.Id = pa.AircraftId
JOIN Pilots AS p ON pa.PilotId = p.Id
WHERE a.FlightHours IS NOT NULL AND a.FlightHours <= 304
ORDER BY a.FlightHours DESC, p.FirstName ASC

--7.	Top 20 Flight Destinations
--Select top 20  flight destinations, where Start day is an even number. 
--Extract DestinationId, Start date, passenger's FullName, AirportName, and TicketPrice. Order the result by TicketPrice descending, then by AirportName ascending.
SELECT TOP(20)
    fd.id AS DestinationId,
    fd.[Start],
    p.FullName,
    ap.AirportName,
    fd.TicketPrice
FROM FlightDestinations AS fd
JOIN Passengers AS p ON p.Id = fd.PassengerId
JOIN Airports AS ap ON ap.Id = fd.AirportId
WHERE DATEPART(DAY, fd.[Start]) % 2 = 0
ORDER BY fd.TicketPrice Desc, ap.AirportName ASC

--8.	Number of Flights for Each Aircraft
--Extract information about all the Aircraft and the count of their FlightDestinations. Display average ticket 
--price (AvgPrice) of each flight destination by the Aircraft, rounded to the second digit. Take only the aircraft with
--at least 2  FlightDestinations. Order the results by count of flight destinations descending, then by the aircraft’s id ascending.

SELECT 
    ac.Id,
    ac.Manufacturer,
    ac.FlightHours,
    Count(fd.Id) AS FlightDestinationsCount,
    ROUND(AVG(fd.TicketPrice), 2) AS AvgPrice
FROM Aircraft AS ac
JOIN FlightDestinations as fd ON ac.Id = fd.AircraftId
GROUP BY ac.id, ac.Manufacturer, ac.FlightHours
HAVING COUNT(fd.Id) >= 2
ORDER BY FlightDestinationsCount DESC, ac.Id

--9.	Regular Passengers
--Extract all passengers, who have flown in more than one aircraft and have an 'a' as the second letter of their full name. Select the full name, 
--the count of aircraft that he/she traveled, and the total sum which was paid.
--Order the result by passenger's FullName.

SELECT
    FullName,
    COUNT(ac.Id) AS CountOfAircraft,
    SUM(fd.TicketPrice) AS TotalPayed
FROM Passengers AS p
JOIN FlightDestinations AS fd ON p.Id = fd.PassengerId
JOIN Aircraft AS ac ON ac.Id = fd.AircraftId
WHERE p.FullName LIKE '_a%'
GROUP BY p.FullName
HAVING COUNT(ac.Id) > 1
ORDER BY p.FullName ASC

--10.	Full Info for Flight Destinations
--Extract information about all flight destinations which Start between hours: 6:00 and 20:00 (both inclusive) and have ticket prices higher than 2500. 
--Select the airport's name, time of the day,  price of the ticket, passenger's full name, aircraft manufacturer, and aircraft model. Order the result by aircraft model ascending.
SELECT 
    ap.AirportName,
    fd.[Start] AS DayTime,
    fd.TicketPrice,
    p.FullName,
    ac.Manufacturer,
    ac.Model
FROM FlightDestinations AS fd
JOIN Airports AS ap ON fd.AirportId = ap.Id
JOIN Passengers AS p ON p.Id = fd.PassengerId
JOIN Aircraft AS ac ON ac.Id = fd.AircraftId
WHERE DATEPART(HOUR, fd.[Start]) BETWEEN 6 AND 20 AND fd.TicketPrice > 2500
ORDER BY ac.Model ASC

--11.	Find all Destinations by Email Address
--Create a user-defined function named udf_FlightDestinationsByEmail(@email) that receives a passenger’s 
--email address and returns the number of flight destinations that the passenger has in the database.

GO

CREATE OR ALTER FUNCTION udf_FlightDestinationsByEmail(@email VARCHAR(50))
RETURNS INT
AS
BEGIN
    RETURN
    (SELECT
        COUNT(fd.Id)
    FROM Passengers AS p
    JOIN FlightDestinations AS fd ON p.Id = fd.PassengerId
    WHERE p.Email = @email)
END

GO

SELECT dbo.udf_FlightDestinationsByEmail ('PierretteDunmuir@gmail.com')
SELECT dbo.udf_FlightDestinationsByEmail('Montacute@gmail.com')
SELECT dbo.udf_FlightDestinationsByEmail('MerisShale@gmail.com')

GO

--12.	Full Info for Airports
--Create a stored procedure, named usp_SearchByAirportName, which accepts the following parameters: airportName(with max length 70)
--Extract information about the airport locations with the given airport name. The needed data is the name of the airport, full name of the passenger, 
--level of the ticket price (depends on flight destination’s ticket price: 'Low'– lower than 400 (inclusive), 'Medium' – between 401 and 1500 (inclusive), 
--and 'High' – more than 1501), manufacturer and condition of the aircraft, and the name of the aircraft type. Order the result by Manufacturer, then by passenger’s full name.
CREATE OR ALTER PROC usp_SearchByAirportName(@airportName VARCHAR(70))
AS
BEGIN

    SELECT
        ap.AirportName,
        p.FullName,
        CASE
            WHEN fd.TicketPrice <= 400 THEN 'Low'
            WHEN fd.TicketPrice BETWEEN 401 AND 1500 THEN 'Medium'
            WHEN fd.TicketPrice >= 1501 THEN 'High'
        END AS LevelOfTickerPrice,
        ac.Manufacturer,
        ac.Condition,
        act.TypeName
    FROM Airports AS ap
    JOIN FlightDestinations AS fd ON ap.Id = fd.AirportId
    JOIN Passengers AS p ON fd.PassengerId = p.Id
    JOIN Aircraft AS ac ON ac.Id = fd.AircraftId
    JOIN AircraftTypes AS act ON ac.TypeId = act.Id
    WHERE ap.AirportName = @airportName
    ORDER BY ac.Manufacturer ASC, p.FullName ASC
END

EXEC usp_SearchByAirportName 'Sir Seretse Khama International Airport'