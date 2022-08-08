CREATE DATABASE Bakery
GO

CREATE TABLE Countries(
    Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(50) UNIQUE
)

CREATE TABLE Customers(
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(25),
    LastName NVARCHAR(25),
    Gender CHAR(1) CHECK (Gender = 'M' OR Gender = 'F'),
    Age INT,
    PhoneNumber CHAR(10),
    CountryId INT REFERENCES Countries(Id)
)

CREATE TABLE Products(
    Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(25) UNIQUE,
    [Description] NVARCHAR(250),
    Recipe NVARCHAR(MAX),
    Price MONEY CHECK(Price >= 0)
)

CREATE TABLE Feedbacks(
    Id INT PRIMARY KEY IDENTITY,
    [Description] NVARCHAR(255),
    Rate DECIMAL(18,2) CHECK(Rate BETWEEN 0 AND 10),
    ProductId INT REFERENCES Products(Id),
    CustomerId INT REFERENCES Customers(Id)
)

CREATE TABLE Distributors(
    Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(25) UNIQUE,
    AddressText NVARCHAR(30),
    Summary NVARCHAR(200),
    CountryId INT REFERENCES Countries(Id)
)

CREATE TABLE Ingredients(
    Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(30),
    [Description] NVARCHAR(200),
    OriginCountryId INT REFERENCES Countries(Id),
    DistributorId INT REFERENCES Distributors(Id)
)

CREATE TABLE ProductsIngredients(
    ProductId INT REFERENCES Products(Id),
    IngredientId INT REFERENCES Ingredients(Id)
    PRIMARY KEY(ProductId, IngredientId)
)

--Section 2. DML

--T02 Insert

INSERT INTO Distributors ([Name], CountryId, AddressText, Summary) VALUES
('Deloitte & Touche', 2, '6 Arch St #9757', 'Customizable neutral traveling'),
('Congress Title', 13, '58 Hancock St', 'Customer loyalty'),
('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery'),
('General Color Co Inc', 21, '6185 Bohn St #72', 'Focus group'),
('Beck Corporation', 23, '21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers (FirstName, LastName, Age, Gender, PhoneNumber, CountryId) VALUES
('Francoise', 'Rautenstrauch', 15, 'M', '0195698399', 5),
('Kendra', 'Loud', 22, 'F', '0063631526', 11),
('Lourdes', 'Bauswell', 50, 'M', '0139037043', 8),
('Hannah', 'Edmison', 18, 'F', '0043343686', 1),
('Tom', 'Loeza', 31, 'M', '0144876096', 23),
('Queenie', 'Kramarczyk', 30, 'F', '0064215793', 29),
('Hiu', 'Portaro', 25, 'M', '0068277755', 16),
('Josefa', 'Opitz', 43, 'F', '0197887645', 17)

--3.	Update
--We’ve decided to switch some of our ingredients to a local distributor. Update the table Ingredients and change the DistributorId of "Bay Leaf", 
--"Paprika" and "Poppy" to 35. Change the OriginCountryId to 14 of all ingredients with OriginCountryId equal to 8.
UPDATE Ingredients
SET DistributorId = 35
WHERE [Name] IN ('Bay Leaf', 'Paprika', 'Poppy')

UPDATE Ingredients
SET OriginCountryId = 14
WHERE OriginCountryId = 8

--4.	Delete
--Delete all Feedbacks which relate to Customer with Id 14 or to Product with Id 5.
DELETE FROM Feedbacks
WHERE CustomerId = 14 OR ProductId = 5

--Section 3. Querying 
--5.	Products by Price
--Select all products ordered by price (descending) then by name (ascending). 
SELECT
    [Name],
    [Price],
    [Description]
FROM Products
ORDER BY [Price] DESC, [Name] ASC

--6.	Negative Feedback
--Select all feedbacks alongside with the customers which gave them. Filter only feedbacks which have rate below 5.0. Order results by ProductId (descending) then by Rate (ascending).
SELECT
    f.ProductId,
    f.Rate,
    f.[Description],
    f.CustomerId,
    c.Age,
    c.Gender
FROM Feedbacks AS f
JOIN Customers AS c ON f.CustomerId = c.Id
WHERE f.Rate < 5
ORDER BY f.ProductId DESC, f.Rate ASC

--7.	Customers without Feedback
--Select all customers without feedbacks. Order them by customer id (ascending).
SELECT
    CONCAT(c.FirstName, ' ',c.LastName) AS CustomerName,
    c.PhoneNumber,
    c.Gender
FROM Customers AS c
LEFT JOIN Feedbacks AS fb ON fb.CustomerId = c.Id
WHERE fb.id IS NULL
ORDER BY c.Id ASC

--8.	Customers by Criteria
--Select customers that are either at least 21 old and contain “an” in their first name or their phone number ends with “38” and are not from Greece. Order by first name (ascending), then by age(descending).
SELECT
    c.FirstName,
    c.Age,
    c.PhoneNumber
FROM Customers AS c
LEFT JOIN Countries AS ctr ON ctr.Id = c.CountryId
WHERE (c.Age >= 21 AND c.FirstName LIKE '%an%') OR (c.PhoneNumber Like '%38' AND ctr.Name <> 'Greece')
ORDER BY c.FirstName ASC, c.Age DESC

--9.	Middle Range Distributors
--Select all distributors which distribute ingredients used in the making process of all products having average rate between 5 and 8 (inclusive). Order by distributor name, ingredient name and product name all ascending.
SELECT
    d.Name AS DistributorName,
    ing.Name AS IngredientName,
    p.Name AS ProductName,
    AVG(fb.Rate) AS AverageRate
FROM Distributors AS d
JOIN Ingredients AS ing ON ing.DistributorId = d.Id
JOIN ProductsIngredients AS pi ON pi.IngredientId = ing.Id
JOIN Products AS p ON pi.ProductId = p.Id
JOIN Feedbacks AS fb ON p.Id = fb.ProductId
GROUP BY  d.[Name], ing.[Name], p.[Name]
HAVING AVG(fb.Rate)BETWEEN 5 AND 8
ORDER BY d.Name ASC, ing.Name, p.Name

--10.	Country Representative
--Select all countries with their most active distributor (the one with the greatest number of ingredients). 
--If there are several distributors with most ingredients delivered, list them all. Order by country name then by distributor name.
SELECT
    CountryName,
    DisributorName
FROM
    (SELECT
    c.Name AS CountryName,
    d.Name AS DisributorName,
    DENSE_RANK() OVER (PARTITION BY c.[Name] ORDER BY COUNT(i.Id) DESC) AS [Ranked]
    FROM Countries AS c
    JOIN Distributors AS d ON c.Id = d.CountryId
    JOIN Ingredients AS i ON i.DistributorId = d.Id
    GROUP BY c.Name, d.Name) 
    AS TEMP
WHERE TEMP.Ranked = 1
ORDER BY temp.CountryName, TEMP.DisributorName

--11.	Customers with Countries
--Create a view named v_UserWithCountries which selects all customers with their countries.
--Required columns:
--•	CustomerName – first name plus last name, with space between them
--•	Age
--•	Gender
--•	CountryName


GO
CREATE VIEW v_UserWithCountries AS
SELECT 
    CONCAT(FirstName, ' ', LastName) AS [CustomerName]
    , c.Age
    , c.Gender
    , ctr.[Name] AS [CountryName] 
FROM Customers AS c
LEFT JOIN Countries AS ctr ON c.CountryId = ctr.Id
GO

SELECT TOP 5 * FROM v_UserWithCountries
ORDER BY Age