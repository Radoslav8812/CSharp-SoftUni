CREATE TABLE Users(
    Id BIGINT PRIMARY KEY IDENTITY(1, 1),
    Username VARCHAR(30) NOT NULL,
    [Password] VARCHAR(26) NOT NULL,
    ProfilePicture VARCHAR (MAX),
    LastLoginTime DATETIME,
    IsDeleted BIT
)

INSERT INTO Users (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Spiro', '1', 'https://avatars.githubusercontent.com/u/28643520?v=4', '1/12/21', 0),
('Kiro', '2', 'https://avatars.githubusercontent.com/u/28643520?v=4', '2/12/21', 0),
('Miro', '3', 'https://avatars.githubusercontent.com/u/28643520?v=4', '3/12/21', 0),
('Stamat', '4', 'https://avatars.githubusercontent.com/u/28643520?v=4', '4/12/21', 0),
('Prahan', '5', 'https://avatars.githubusercontent.com/u/28643520?v=4', '5/12/21', 0)

SELECT * FROM Users