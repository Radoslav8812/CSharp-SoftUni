CREATE TABLE People(
    Id INT IDENTITY (1, 1) PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    Picture VARBINARY (MAX),
    Height DECIMAL (6, 2),
    [Weight] DECIMAL (6, 2),
    Gender CHAR(1) NOT NULL,
    Birthdate DATETIME2 NOT NULL,
    Biography NVARCHAR(MAX)
)

INSERT INTO People VALUES
('Spiro', NULL, 1.20, 120, 'm', '1977-12-12', 'very good lad'),
('Kiro', NULL, 1.80, 75, 'm', '1975-12-15', 'bugistata'),
('Emba', NULL, 1.75, 85, 'm', '1989-05-07', 'Koloezdacha'),
('Emporuo', NULL, 1.67, 77, 'm', '1985-11-11', 'Razporuo'),
('Brizent', NULL, 1.50, 85, 'm', '1978-12-13', 'Spiridon Spiridonov')