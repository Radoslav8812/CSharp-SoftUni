CREATE DATABASE [Minions]

USE [Minions]

/* Create table "Minions" */
CREATE TABLE [Minions]
(
    [Id] INT PRIMARY KEY,
    [Name] NVARCHAR(50),
    [Age] INT
)

/* Create table "Towns" */
CREATE TABLE [Towns]
(
    [Id] INT PRIMARY KEY,
    [Name] NVARCHAR(50)
)

/* Add aditional column to "Minions" table */
ALTER TABLE [Minions]
ADD [TownId] INT

/* Column "TownId" reference "Id" in "Towns" table */
ALTER TABLE [Minions]
ADD FOREIGN KEY (TownId) REFERENCES [Towns](Id)

/* Insert Data*/
INSERT INTO [Towns] VALUES
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

/* Insert Data*/
INSERT INTO [Minions] VALUES
(1, 'Spiro', 22, 1),
(2, 'Kiro', 22, 2),
(3, 'Miro', Null, 3)

SELECT * FROM [Minions]

/* Delete whole inserted data */
DELETE FROM [Minions]

/* Create table "Users" */
CREATE TABLE [Users]
(
    [Id] INT PRIMARY KEY IDENTITY,
    [UserName] VARCHAR(30) NOT NULL,
    [Password] VARCHAR(26) NOT NULL,
    [ProfilePicture] VARCHAR(MAX),
    [LastLoginTime] DATETIME2,
    [IsDeleted] BIT
)

INSERT INTO [Users] VALUES
('Spiro', '12345', 'pic1', '04-02-2005', 0),
('Spiro', '123', 'pi2c', '04-02-2006', 0),
('Spiro', '1234567', 'p3ic', '04-02-2008', 0),
('Spiro', '1111111', 'pi4c', '04-02-2009', 0),
('Spiro', '4444444', 'pi5c', '04-02-2004', 0)

SELECT * FROM [Users]