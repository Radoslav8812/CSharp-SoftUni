CREATE DATABASE WMS
GO

USE WMS

CREATE TABLE Clients
(
ClientId INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR (50) NOT NULL,
Phone CHAR (12) NOT NULL
)

CREATE TABLE Mechanics
(
MechanicId INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR (50) NOT NULL,
[Address] VARCHAR(255) NOT NULL
)

CREATE TABLE Models
(
ModelId INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) UNIQUE NOT NULL,
)

CREATE TABLE Jobs
(
JobId INT PRIMARY KEY IDENTITY,
ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL,
[Status] VARCHAR (11) DEFAULT 'Pending' CHECK ([Status] IN('Pending', 'In Progress' ,'Finished')) NOT NULL,
ClientId INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL, 
MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
IssueDate DATE NOT NULL,
FinishDate DATE
)

CREATE TABLE Orders
(
OrderId INT PRIMARY KEY IDENTITY,
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
IssueDate DATE,
Delivered BIT DEFAULT 0  NOT NULL
)

CREATE TABLE Vendors
(
VendorId INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts
(
PartId INT PRIMARY KEY IDENTITY,
SerialNumber VARCHAR(50) UNIQUE NOT NULL,
[Description] VARCHAR(255),
Price MONEY CHECK (Price >0 AND Price < 9999.99) NOT NULL,
VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId) NOT NULL,
StockQty INT CHECK(StockQty >=0) DEFAULT 0 NOT NULL
)

CREATE TABLE OrderParts
(
OrderId INT FOREIGN KEY REFERENCES Orders(OrderId)NOT NULL,
PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
PRIMARY KEY(OrderId, PartId),
Quantity INT CHECK(Quantity > 0) DEFAULT 1 NOT NULL
)

CREATE TABLE PartsNeeded
(
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
PRIMARY KEY (JobId, PartId),
Quantity INT CHECK(Quantity > 0) DEFAULT 1 NOT NULL
)
--Section 2. DML

--T02 Insert

INSERT INTO Clients VALUES
('Teri',	'Ennaco',	'570-889-5187'),
('Merlyn',	'Lawler',	'201-588-7810'),
('Georgene', 'Montezuma', '925-615-5185'),
('Jettie',	'Mconnell',	'908-802-3564'),
('Lemuel',	'Latzke',	'631-748-6479'),
('Melodie',	'Knipp',	'805-690-1682'),
('Candida',	'Corbley',	'908-275-8357')

INSERT INTO Parts (SerialNumber, [Description], Price, VendorId) VALUES
('WP8182119', 'Door Boot Seal',	117.86,	2),
('W10780048', 'Suspension Rod',	42.81,	1),
('W10841140', 'Silicone Adhesive',  6.77,	4),
('WPY055980', 'High Temperature Adhesive', 13.94,	3)

--3.	Update
--Assign all Pending jobs to the mechanic Ryan Harnos (look up his ID manually, there is no need to use table joins) and change their status to 'In Progress'.
SELECT
    *
FROM Mechanics
WHERE FirstName = 'Ryan' AND LastName = 'Harnos'

SELECT
    *
FROM Jobs

UPDATE Jobs
    SET MechanicId = 3
    WHERE [Status] = 'Pending'

UPDATE Jobs
    SET [Status] = 'In Progress'
    WHERE [Status] = 'Pending'

--4.	Delete
--Cancel Order with ID 19 – delete the order from the database and all associated entries from the mapping table.

DELETE
FROM OrderParts
WHERE OrderID = 19

DELETE
FROM Orders
WHERE OrderId = 19

--5.	Mechanic Assignments
--Select all mechanics with their jobs. Include job status and issue date. Order by mechanic Id, issue date, job Id (all ascending).
SELECT
    m.FirstName + ' ' + m.LastName AS Mechanic,
    j.[Status],
    j.IssueDate
FROM Mechanics AS m
JOIN Jobs AS j ON j.MechanicId = m.MechanicId
ORDER BY m.MechanicId, j.IssueDate, j.JobId

--6.	Current Clients
--Select the names of all clients with active jobs (not Finished). Include the status of the job and how many days it’s been since it was submitted. 
--Assume the current date is 24 April 2017. Order results by time length (descending) and by client ID (ascending).
SELECT
    c.FirstName + ' ' + c.LastName AS Client,
    DATEDIFF(DAY, j.IssueDate, '24 April 2017') AS [Days goint],
    j.[Status]
FROM Clients AS c
JOIN Jobs AS j ON c.ClientId = j.ClientId
WHERE j.[Status] != 'Finished'
ORDER BY [Days goint] DESC, c.ClientId ASC


--7.	Mechanic Performance
--Select all mechanics and the average time they take to finish their assigned jobs. Calculate the average as an integer. Order results by mechanic ID (ascending).
SELECT
    m.FirstName + ' ' + m.LastName AS Mechanic,
    AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Average Days]
FROM Mechanics AS m
JOIN Jobs AS j ON m.MechanicId = j.MechanicId
GROUP BY m.FirstName + ' ' + m.LastName, m.MechanicId
ORDER BY m.MechanicId ASC

--8.	Available Mechanics
--Select all mechanics without active jobs (include mechanics which don’t have any job assigned or all of their jobs are finished). Order by ID (ascending).
SELECT
    m.FirstName + ' ' + m.LastName AS [Mechanic]
FROM Mechanics AS m
JOIN Jobs AS j ON m.MechanicId = j.MechanicId
WHERE m.MechanicId NOT IN(
                            SELECT
                                MechanicId
                            FROM Jobs
                            WHERE [Status] = 'In Progress'
)
GROUP BY m.FirstName + ' ' + m.LastName, m.MechanicId
ORDER BY m.MechanicId ASC

--9.	Past Expenses
--Select all finished jobs and the total cost of all parts that were ordered for them. Sort by total cost of parts ordered (descending) and by job ID (ascending).

SELECT 
    j.JobId,
    ISNULL(SUM(p.Price * op.Quantity),0) AS [Total] 
FROM Jobs AS j
LEFT JOIN Orders AS o ON j.JobId = o.JobId
LEFT JOIN OrderParts AS op ON o.OrderId = op.OrderId
LEFT JOIN Parts AS p ON op.PartId = p.PartId
WHERE j.[Status] = 'Finished'
GROUP BY j.JobId
ORDER BY Total DESC, j.JobId


--10.	Missing Parts
--List all parts that are needed for active jobs (not Finished) without sufficient quantity in stock and in 
--pending orders (the sum of parts in stock and parts ordered is less than the required quantity). Order them by part ID (ascending).
SELECT * FROM
(SELECT
    p.PartId,
    p.[Description],
    pn.Quantity AS [Required],
    p.StockQty AS [In Stock],
    ISNULL(op.Quantity, 0) AS [Ordered]
FROM Jobs AS j
LEFT JOIN PartsNeeded AS pn On j.JobId = pn.JobId
LEFT JOIN Parts AS p ON p.PartId = pn.PartId
LEFT JOIN Orders AS o ON o.JobId = j.JobId
LEFT JOIN OrderParts AS op ON o.OrderId = op.OrderId
WHERE j.[Status] <> 'Finished' AND (o.Delivered = 0 Or o.Delivered IS NULL)) 
    AS PartsQtytySubquery
WHERE [Required] > [In Stock] + Ordered
ORDER BY PartId

--11.	Place Order
--Your task is to create a user defined procedure (usp_PlaceOrder) which accepts job ID, part serial number and   quantity and creates an order with the specified parameters. 
--If an order already exists for the given job that and the order is not issued (order’s issue date is NULL), add the new product to it. If the part is already listed in the order, 
--add the quantity to the existing one.
--When a new order is created, set it’s IssueDate to NULL.
--Limitations:
--•	An order cannot be placed for a job that is Finished; error message ID 50011 "This job is not active!"
--•	The quantity cannot be zero or negative; error message ID 50012 "Part quantity must be more than zero!"
--•	The job with given ID must exist in the database; error message ID 50013 "Job not found!"
--•	The part with given serial number must exist in the database ID 50014 "Part not found!"
I--f any of the requirements aren’t met, rollback any changes to the database you’ve made and throw an exception with the appropriate message and state 1. 



GO
--12.	Cost Of Order
--Create a user defined function (udf_GetCost) that receives a job’s ID and returns the total cost of all parts that were ordered for it. Return 0 if there are no orders.
CREATE FUNCTION udf_GetCost(@jobId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN

    RETURN ISNULL((SELECT SUM(p.Price * op.Quantity)
    FROM Jobs AS j
    JOIN Orders AS o ON j.JobId = o.JobId
    JOIN OrderParts AS op ON op.OrderId = o.OrderId
    JOIN Parts AS p On op.PartId = p.PartId
    WHERE j.JobId = @jobId), 0)

END
    
GO

SELECT dbo.udf_GetCost(1) --91.86
SELECT dbo.udf_GetCost(3) --40.97
SELECT dbo.udf_GetCost(6) --27.15