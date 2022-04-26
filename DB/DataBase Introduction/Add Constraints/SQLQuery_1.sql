CREATE TABLE Users(
    Id BIGINT PRIMARY KEY IDENTITY(1, 1),
    Username VARCHAR(30) NOT NULL,
    [Password] VARCHAR(26) NOT NULL,
    ProfilePicture VARCHAR (MAX),
    LastLoginTime DATETIME,
    IsDeleted BIT
)

INSERT INTO Users (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Spiro', '213231', 'https://avatars.githubusercontent.com/u/28643520?v=4', '1/12/21', 0),
('Kiro', '23213321', 'https://avatars.githubusercontent.com/u/28643520?v=4', '2/12/21', 0),
('Miro', 'dsad23sedsa', 'https://avatars.githubusercontent.com/u/28643520?v=4', '3/12/21', 0),
('Stamat', 'dsadsadsads', 'https://avatars.githubusercontent.com/u/28643520?v=4', '4/12/21', 0),
('Prahan', 'dsadsdsadsa', 'https://avatars.githubusercontent.com/u/28643520?v=4', '5/12/21', 0)

ALTER TABLE [dbo].[Users] DROP CONSTRAINT [PK__Users__3214EC074E961E90] WITH ( ONLINE = OFF )
GO

ALTER TABLE Users
ADD CONSTRAINT PK__Usersname PRIMARY KEY (Id, Username)

ALTER TABLE Users
ADD CONSTRAINT CH_PasswordIsAtLeastFiveSimbols CHECK (LEN([Password]) > 5)

ALTER TABLE Users
ADD CONSTRAINT DF_LastLoginTime DEFAULT GETDATE() FOR LastLoginTime