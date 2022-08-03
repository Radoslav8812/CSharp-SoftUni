--Section 1. DDL (30 pts)
CREATE TABLE Users(
    Id INT PRIMARY KEY IDENTITY,
    UserName VARCHAR(30) NOT NULL,
    [Password] VARCHAR(30) NOT NULL,
    Email VARCHAR(50) NOT NULL
)

CREATE TABLE Repositories(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(50)
)

CREATE TABLE RepositoriesContributors(
    RepositoryId INT REFERENCES Repositories([Id]) NOT NULL,
    ContributorId INT REFERENCES Users([Id]) NOT NULL,

    PRIMARY KEY(RepositoryId, ContributorId)
)

CREATE TABLE Issues(
    Id INT PRIMARY KEY IDENTITY,
    TItle VARCHAR(255) NOT NULL,
    IssueStatus VARCHAR(6) NOT NULL,
    RepositoryId INT REFERENCES Repositories([Id]) NOT NULL,
    AssigneeId INT REFERENCES Users([Id]) NOT NULL
)

CREATE TABLE Commits(
    Id INT PRIMARY KEY IDENTITY,
    [Message] VARCHAR(255) NOT NULL,
    IssueId INT REFERENCES Issues([Id]),
    RepositoryId INT REFERENCES Repositories([Id]) NOT NULL,
    ContributorId INT REFERENCES Users([Id]) NOT NULL
)

CREATE TABLE Files(
    Id INT PRIMARY KEY IDENTITY,
    [Name] VARCHAR(100) NOT NULL,
    Size DECIMAL(18,2) NOT NULL,
    ParentId INT REFERENCES Files([Id]),
    CommitId INT REFERENCES Commits([Id]) NOT NULL
)


--Section 2. DML (10 pts)
INSERT INTO Files VALUES
('Trade.idk', 2598.0, 1, 1),
('menu.net', 9238.31, 2, 2),
('Administrate.soshy', 1246.93, 2, 2),
('Controller.php', 7353.15, 4, 4),
('Find.java', 9957.86, 5, 5),
('Controller.json', 14034.87, 3, 6),
('Operate.xix', 7662.92, 7, 7)

INSERT INTO Issues VALUES
('Critical Problem with HomeController.cs file', 'open', 1, 1),
('Typo fix in Judge.html', 'open', 4, 3),
('Implement documentation for UsersService.cs', 'closed', 8, 2),
('Unreachable code in Index.cs', 'open', 9, 8)

--3.	Update
UPDATE Issues
SET IssueStatus = 'Closed'
WHERE AssigneeId = 6

--4.	Delete
DELETE FROM RepositoriesContributors
WHERE RepositoryId = (SELECT Id FROM Repositories
WHERE [Name] = 'Softuni-Teamwork')

DELETE FROM Issues
WHERE RepositoryId = (SELECT Id FROM Repositories
WHERE [Name] = 'Softuni-Teamwork')


--Section 3. Querying (40 pts)
--5.	Commits
SELECT
    Id,
    [Message],
    RepositoryId,
    ContributorId
FROM Commits
ORDER BY Id, [message], RepositoryId, ContributorId

--6.	Front-end
SELECT
    Id,
    [Name],
    [Size]
FROM Files
WHERE [Size] > 1000 AND [Name] LIKE '%html%'
ORDER BY [Size] DESC, Id, [Name]

--7.	Issue Assignment
SELECT
    i.Id,
    u.UserName + ' : ' + i.TItle AS IssueAssignee
FROM Issues AS i
JOIN Users AS u ON u.Id = i.AssigneeId
ORDER BY i.Id DESC, IssueAssignee


--8.	Single Files
SELECT
    Id,
    [Name],
    CONCAT(CONVERT(VARCHAR, Size), 'KB') AS [Size] 
FROM Files
WHERE Id NOT IN (SELECT ParentId FROM Files
                WHERE ParentId IS NOT NULL)
ORDER BY Id, [Name], Size DESC

--9.	Commits in Repositories
SELECT TOP(5)
    r.Id,
    r.[Name],
    COUNT(c.Id) AS Commits
FROM Commits AS [c]
JOIN Repositories AS r ON c.RepositoryId = r.Id
JOIN RepositoriesContributors AS rc ON r.Id = rc.RepositoryId
GROUP BY r.Id, r.[Name]
ORDER BY Commits DESC, r.Id, r.[Name]

--10.	Average Size
SELECT
    u.UserName,
    AVG(f.[Size]) AS Size
FROM Users AS [u]
JOIN Commits as c ON u.Id = c.ContributorId
JOIN Files AS f ON c.Id = f.CommitId
GROUP BY u.Username
ORDER BY Size DESC, u.Username ASC

GO

--11
CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(30))
RETURNS INT
AS
BEGIN
    DECLARE @UserId INT = 
        (SELECT
        Id
        FROM Users
        WHERE UserName = @username)
    
    DECLARE @Commits INT =
    (
        SELECT COUNT(Id)
        FROM Commits
        WHERE ContributorId = @UserId
    )
    RETURN @Commits
END

SELECT dbo.udf_AllUserCommits('UnderSinduxrein')

GO

CREATE PROCEDURE usp_SearchForFiles(@fileExtension VARCHAR(98))
AS
BEGIN
    SELECT
        Id,
        [Name],
        CONCAT(Size, 'KB') AS [Size]
        FROM Files
        WHERE [Name] LIKE CONCAT('%[.]', @fileExtension)
        ORDER BY Id ASC, [Name] ASC, [Size] DESC
END

    EXEC usp_SearchForFiles 'txt'

GO