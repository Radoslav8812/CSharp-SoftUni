CREATE DATABASE CigarShop
GO

--1
CREATE TABLE [Sizes] (
    [Id] INT PRIMARY KEY IDENTITY,
    [Length] INT NOT NULL CHECK ([Length] BETWEEN 10 AND 25),
    [RingRange] DECIMAL (18,2) NOT NULL CHECK ([RingRange] BETWEEN 1.5 AND 7.5)
)

CREATE TABLE [Tastes](
    [Id] INT PRIMARY KEY IDENTITY,
    [TasteType] VARCHAR (20) NOT NULL,
    [TasteStrength] VARCHAR(15) NOT NULL,
    [ImageURL] NVARCHAR(100) Not NULL
)

CREATE TABLE [Brands](
    [Id] INT PRIMARY KEY IDENTITY,
    [BrandName] VARCHAR (80) UNIQUE NOT NULL,
    [BrandDescription] VARCHAR(MAX)
)

CREATE TABLE [Cigars](
    [Id] INT PRIMARY KEY IDENTITY,
    [CigarName] VARCHAR(80) NOT NULL,
    [BrandId] INT REFERENCES [Brands]([Id]) NOT NULL,
    [TastId] INT REFERENCES [Tastes]([Id]) NOT NULL,
    [SizeId] INT REFERENCES [Sizes]([Id]) NOT NULL,
    [PriceForSingleCigar] MONEY NOT NULL,
    [ImageURL] NVARCHAR(100) NOT NULL
)

CREATE TABLE [Addresses](
    [Id] INT PRIMARY KEY IDENTITY,
    [Town] VARCHAR(30) NOT NULL,
    [Country] NVARCHAR(30) NOT NULL,
    [Streat] NVARCHAR(100) NOT NULL,
    [ZIP] VARCHAR(20)
)

CREATE TABLE [Clients](
    [Id] InT PRIMARY KEY IDENTITY,
    [FirstName] NVARCHAR(30) NOT NULL,
    [LastName] NVARCHAR(30) NOT NULL,
    [Email] NVARCHAR(50) NOT NULL,
    [AddressId] INT REFERENCES [Addresses]([Id])
)

CREATE TABLE [ClientsCigars](
    [ClientId] INT REFERENCES Clients([Id]),
    [CigarId] INT REFERENCES Cigars([Id]),

    PRIMARY KEY ([ClientId], [CigarId])
)

--2

--      Querying
--
--5.    Cigars by Price
SELECT
    [CigarName],
    [PriceForSingleCigar],
    [ImageURL]
FROM [Cigars]
ORDER BY [PriceForSingleCigar] ASC, [CigarName] DESC

--6.	Cigars by Taste
SELECT
	c.Id,
	c.CigarName,
	c.PriceForSingleCigar,
	t.TasteType,
	t.TasteStrength
FROM [Cigars] AS [c]
JOIN [Tastes] AS [t] ON [c].[TastId] = [t].[Id]
WHERE t.[TasteType] IN ('Woody', 'Earthy')
ORDER BY PriceForSingleCigar DESC


--7.	Clients without Cigars
SELECT
    Id,
    FirstName + ' ' + LastName AS ClientName,
    Email
FROM Clients
WHERE NOT EXISTS (
    SELECT ClientId
    FROM ClientsCigars 
    WHERE ClientId = Id)
ORDER BY ClientName ASC

--8.	First 5 Cigars
SELECT TOP(5)
    CigarName,
    PriceForSingleCigar,
    ImageURL
FROM Cigars AS c
JOIN Sizes AS sz ON c.SizeId = sz.Id
WHERE sz.Length >= 12 AND (CigarName LIKE '%ci%' OR c.PriceForSingleCigar > 50) AND sz.RingRange > 2.55
ORDER BY c.CigarName ASC, c.PriceForSingleCigar DESC

--9.	Clients with ZIP Codes
SELECT
    cli.FirstName + ' ' + cli.LastName AS [FullName],
    a.Country,
    a.ZIP,
    CONCAT ('$', 
    (SELECT MAX(PriceForSingleCigar)
    FROM Cigars AS cig
    JOIN ClientsCigars as cc ON cig.Id = cc.CigarId AND cc.ClientId = cli.Id)) AS CigarPrice
FROM Clients AS cli
JOIN Addresses AS a ON cli.AddressId = a.Id
WHERE ISNUMERIC(a.ZIP) = 1
ORDER BY FullName ASC

--10.	Cigars by Size
SELECT
    c.LastName,
    AVG(sz.Length) AS CiagrLength,
    CEILING(AVG(sz.RingRange)) AS CiagrRingRange
FROM Clients c
JOIN ClientsCigars cc ON c.Id = cc.ClientId
JOIN Cigars cig On cc.CigarId = cig.Id
JOIN Sizes sz ON cig.SizeId = sz.Id
GROUP BY c.LastName
ORDER BY CiagrLength DESC

GO

--11.	Client with Cigars
CREATE OR ALTER FUNCTION udf_ClientWithCigars(@name NVARCHAR(30))
RETURNS INT AS
BEGIN
    DECLARE @cigarCount INT;
        SET @cigarCount = (SELECT COUNT(*) FROM ClientsCigars
        WHERE ClientId IN (SELECT Id FROM Clients WHERE FirstName = @name));
    RETURN @cigarCount;
END

SELECT dbo.udf_ClientWithCigars('Betty')

GO
--12.	Search for Cigar with Specific Taste
CREATE OR ALTER PROCEDURE usp_SearchByTaste(@taste VARCHAR(20))
AS
BEGIN
SELECT
    CigarName,
    CONCAT('$', c.PriceForSingleCigar) AS Price,
    TasteType,
    b.BrandName,
    CONCAT(s.Length, ' ', 'cm') AS CigarLength,
    CONCAT(s.RingRange, ' ', 'cm') AS CigarRingRange
FROM Cigars AS c
JOIN Tastes AS t ON c.TastId = t.id
JOIN Sizes AS s ON c.SizeId = s.Id
Join Brands AS b ON c.BrandId = b.Id
WHERE t.TasteType = @taste
ORDER BY CigarLength ASC, RingRange DESC
END

EXEC usp_SearchByTaste 'Woody'