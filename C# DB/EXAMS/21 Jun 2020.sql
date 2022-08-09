
CREATE TABLE Cities(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    [Name] NVARCHAR(20),
    CountryCode CHAR(2) NOT NULL
)

CREATE TABLE Hotels(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    [Name] NVARCHAR(30) NOT NULL,
    CityId INT REFERENCES Cities(Id) NOT NULL,
    EmployeeCount INT NOT NULL,
    BaseRate DECIMAL (18, 2)
)

CREATE TABLE Rooms(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    Price DECIMAL (18, 2) NOT NULL,
    [Type] NVARCHAR(20) NOT NULL,
    Beds INT NOT NULL,
    HotelId INT REFERENCES Hotels(Id) NOT NULL
)

CREATE TABLE Trips(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    RoomId INT REFERENCES Rooms(Id) NOT NULL,
    BookDate DATE NOT NULL,
    ArrivalDate DATE NOT NULL,
    ReturnDate DATE NOT NULL,
    CancelDate DATE,
    CHECK(BookDate < ArrivalDate),
    CHECK(ArrivalDate < ReturnDate)
)

CREATE TABLE Accounts(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(20),
    LastName NVARCHAR(50) NOT NULL,
    CityId INT REFERENCES Cities(Id) NOT NULL,
    BirthDate DATE NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE
)

CREATE TABLE AccountsTrips(
    AccountId INT REFERENCES Accounts(Id) NOT NULL,
    TripId  INT REFERENCES Trips(Id) NOT NULL,
    PRIMARY KEY (AccountId, TripId),
    Luggage INT NOT NULL,
    CHECK(Luggage >= 0)
)

--2. Insert
--Insert some sample data into the database. Write a query to add the following records into the corresponding tables. All Ids should be auto-generated.
INSERT INTO Accounts VALUES
('John', 'Smith', 'Smith', 34, '1975-07-21', 'j_smith@gmail.com'),
('Gosho', NULL,	'Petrov', 11, '1978-05-16',	'g_petrov@gmail.com'),
('Ivan', 'Petrovich', 'Pavlov',	59,	'1849-09-26', 'i_pavlov@softuni.bg'),
('Friedrich', 'Wilhelm', 'Nietzsche', 2, '1844-10-15', 'f_nietzsche@softuni.bg')

INSERT INTO Trips VALUES
(101, '2015-04-12',	'2015-04-14', '2015-04-20', '2015-02-02'),
(102, '2015-07-07',	'2015-07-15', '2015-07-22',	'2015-04-29'),
(103, '2013-07-17',	'2013-07-23', '2013-07-24',	NULL),
(104, '2012-03-17',	'2012-03-31', '2012-04-01',	'2012-01-10'),
(109, '2017-08-07',	'2017-08-28', '2017-08-29',	NULL)

--3. Update
--Make all rooms’ prices 14% more expensive where the hotel ID is either 5, 7 or 9.
UPDATE Rooms
SET Price *= 1.14
WHERE HotelId IN (5, 7 , 9)

--4. Delete
-- Delete all of Account ID 47’s account’s trips from the mapping table.
DELETE FROM AccountsTrips
WHERE AccountId = 47

--Section 3. Querying (40 pts)
--5. EEE-Mails
--Select accounts whose emails start with the letter “e”. Select their first and last name, their birthdate in the format "MM-dd-yyyy", their city name, and their Email.
--Order them by city name (ascending)
SELECT
    a.FirstName,
    a.LastName,
    FORMAT(a.BirthDate, 'MM-dd-yyyy') AS BirthDate,
    c.Name AS HomeTown,
    a.Email
FROM Accounts AS a
JOIN Cities AS c ON a.CityId = c.Id
WHERE a.Email LIKE 'e%'
ORDER BY c.Name ASC

--6. City Statistics
--Select all cities with the count of hotels in them. Order them by the hotel count (descending), 
--then by city name. Do not include cities, which have no hotels in them.
SELECT
    c.Name,
    COUNT(h.Id) AS Hotels
FROM Cities AS c
JOIN Hotels AS h ON h.CityId = c.Id
WHERE h.Id IS NOT NULL
GROUP BY c.Name
ORDER BY Hotels DESC, c.Name ASC


--7. Longest and Shortest Trips
--Find the longest and shortest trip for each account, in days. Filter the results to accounts with no middle name and trips, which are not cancelled (CancelDate is null).
--Order the results by Longest Trip days (descending), then by Shortest Trip (ascending).
SELECT
    ac.AccountId,
    (SELECT FirstName + ' ' + LastName FROM Accounts WHERE Id = AccountId) AS FullName,
    MAX(DATEDIFF(DAY, ArrivalDate, ReturnDate)) AS LongestTrip,
    MIN(DATEDIFF(DAY, ArrivalDate, ReturnDate)) AS ShortestTrip
FROM Accounts AS a
JOIN AccountsTrips AS ac ON a.Id = ac.AccountId
JOIN Trips AS t ON ac.TripId = t.id
WHERE a.MiddleName IS NULL AND CancelDate IS NULL
GROUP BY AccountId
ORDER BY LongestTrip DESC, ShortestTrip ASC

--8. Metropolis
--Find the top 10 cities, which have the most registered accounts in them. 
--Order them by the count of accounts (descending).
SELECT TOP(10)
    c.Id,
    c.Name,
    c.CountryCode,
    COUNT(a.Id) AS Accounts
FROM Cities AS c
JOIN Accounts AS a ON c.Id = a.CityId
GROUP BY c.Id, c.Name, c.CountryCode
ORDER BY COUNT(a.Id) DESC

--9. Romantic Getaways
--Find all accounts, which have had one or more trips to a hotel in their hometown.
--Order them by the trips count (descending), then by Account ID.
SELECT 
    a.Id,
    a.Email,
    c.Name,
    COUNT(t.Id) AS Trips
FROM Accounts AS a
JOIN Cities AS c ON c.id = a.CityId
JOIN AccountsTrips AS [at] ON a.Id = [at].[AccountId]
JOIN Trips AS t ON [at].[TripId] = t.Id
JOIN Rooms AS r ON t.RoomId = r.Id
JOIN Hotels AS h ON h.Id = r.HotelId
WHERE a.CityId = h.CityId
GROUP BY a.Id, a.Email, c.[Name]
ORDER BY Trips DESC, a.Id

--10. GDPR Violation
--Retrieve the following information about each trip:
--•	Trip ID
--•	Account Full Name
--•	From – Account hometown
--•	To – Hotel city
--•	Duration – the duration between the arrival date and return date in days. If a trip is cancelled, the value is “Canceled”
--Order the results by full name, then by Trip ID.

SELECT
    t.Id,
    CONCAT(a.FirstName,' ', ISNULL(a.MiddleName + ' ', ''), a.LastName) AS [Full Name],
    c1.Name AS [From],
    c2.Name AS [To],
    CASE
    WHEN t.CancelDate IS NOT NULL THEN 'Canceled'
    ELSE CONCAT(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate), ' days')
    END AS [Duration]
FROM Trips AS t
JOIN AccountsTrips AS act ON t.Id =act.TripId
JOIN Accounts AS a ON act.AccountId = a.Id
JOIN Cities AS c1 ON a.CityId = c1.Id
JOIN Rooms AS r ON r.Id = t.RoomId
JOIN Hotels AS h ON r.HotelId  = h.Id
JOIN Cities AS c2 ON h.CityId = c2.Id
ORDER BY [Full Name], t.Id

--T11 Available Room

GO
CREATE OR ALTER FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATETIME, @People INT)
RETURNS VARCHAR(MAX)
AS
BEGIN

DECLARE @OccupiedRooms TABLE(Id INT) 
INSERT INTO  @OccupiedRooms 
SELECT r.Id FROM Rooms AS r
JOIN Trips AS t ON r.Id = t.RoomId
WHERE r.HotelId = @HotelId AND @Date> t.ArrivalDate AND @Date < t.ReturnDate AND t.CancelDate IS NULL
RETURN
ISNULL((SELECT TOP 1
CONCAT('Room ', r.Id, ': ', r.[Type], ' (', r.Beds, ' beds) - $', (h.BaseRate + r.Price) * @People)
FROM Rooms AS r
LEFT JOIN Trips AS t ON r.Id= t.RoomId
JOIN Hotels AS h ON r.HotelId = h.Id
WHERE r.HotelId = @HotelId AND r.Beds >=  @People AND r.Id NOT IN (SELECT * FROM @OccupiedRooms)
ORDER BY (h.BaseRate + r.Price) * @People  DESC), 'No rooms available');
END

GO
SELECT dbo.udf_GetAvailableRoom(112, '2011-12-17', 2) --Room 211: First Class (5 beds) - $202.80 
SELECT dbo.udf_GetAvailableRoom(94, '2015-07-26', 3) --No rooms available


--T12 Switch Room

SELECT r.HotelId FROM Trips AS t
JOIN Rooms AS r ON t.RoomId = r.Id
WHERE t.Id=10 --6 (Trip's HotelId)

SELECT Beds FROM Rooms
WHERE Id = 11 --3 (TargetRoom Beds)

SELECT COUNT(*) 
FROM AccountsTrips
WHERE TripId = 10 --2 (Number of Trip's Accounts)

GO
CREATE PROC usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
BEGIN
DECLARE @TripHotelId  INT = (SELECT r.HotelId FROM Trips AS t
JOIN Rooms AS r ON t.RoomId = r.Id WHERE t.Id=@TripId)
DECLARE @TargetRoomHotelId INT = (SELECT HotelId FROM Rooms WHERE Id = @TargetRoomId)
DECLARE @TargetRoomBeds INT = (SELECT Beds FROM Rooms WHERE Id = @TargetRoomId)
DECLARE @NumberOfTripAccounts INT = (SELECT COUNT(*) FROM AccountsTrips 
WHERE TripId = @TripId)
IF @TripHotelId != @TargetRoomHotelId
 BEGIN
 RAISERROR ('Target room is in another hotel!' , 16,1)
 END
 IF @TargetRoomBeds < @NumberOfTripAccounts
  BEGIN
  RAISERROR ('Not enough beds in target room!', 16,2)
  END
 UPDATE Trips
 SET RoomId = @TargetRoomId
 WHERE Id  =@TripId
END

EXEC usp_SwitchRoom 10, 11

SELECT RoomId FROM Trips WHERE Id = 10 --11

EXEC usp_SwitchRoom 10, 7 --Target room is in another hotel!

EXEC usp_SwitchRoom 10, 8 -- Not enough beds in target room!