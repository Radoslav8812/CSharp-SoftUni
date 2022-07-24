CREATE DATABASE CigarShop
GO


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
