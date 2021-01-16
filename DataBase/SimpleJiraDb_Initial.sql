USE SimpleJiraDB;
GO

/***** LIST OF ALL USER DEFINED TABLES ******/
select *
from sys.all_objects
where [type]= 'U'
;
go

/***** LIST OF ALL THE CONSTRIANTS ******/
SELECT OBJECT_NAME(OBJECT_ID) AS NameofConstraint,
SCHEMA_NAME(schema_id) AS SchemaName,
OBJECT_NAME(parent_object_id) AS TableName,
type_desc AS ConstraintType
FROM sys.objects
WHERE type_desc LIKE '%CONSTRAINT';
GO


/***** DISABLE ALL THE CONSTRAINT BEFORE DROP TABLES ******/
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
    + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
    ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.foreign_keys;

PRINT @sql;
EXEC sp_executesql @sql;


/***** DROP TABLES ******/
DROP TABLE Projects;
DROP TABLE Sprints;
DROP TABLE UserStories;
DROP TABLE Issues;
DROP TABLE Users;
DROP TABLE Teams;
GO

/***** Table No. 1 - Teams ****/
CREATE TABLE Teams
(
	TeamId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(50) NOT NULL,
	CONSTRAINT PK_Teams PRIMARY KEY CLUSTERED (TeamId ASC)
)
;
go

ALTER TABLE Teams
ADD UNIQUE (Name);
go

/***** Table No. 2 - Users ****/
CREATE TABLE Users
(
	UserId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	LoginName NVARCHAR(40) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PWDEncrypted NVARCHAR(200) NOT NULL,
	Role nvarchar(50) NOT NULL,
	EMAIL NVARCHAR(50) NOT NULL,
	TeamId int NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (UserId ASC)
)
;
go

ALTER TABLE Users
ADD UNIQUE (LoginName);
go

ALTER TABLE Users
ADD
	CONSTRAINT FK_Users_Teams FOREIGN KEY (TeamId) REFERENCES Teams(TeamId)
;
go

/***** Table No. 3 - Projects ****/
CREATE TABLE Projects
(
	ProjectId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(50) NOT NULL,
	TeamId INTEGER NOT NULL;
	CONSTRAINT PK_Projects PRIMARY KEY CLUSTERED (ProjectId ASC)
)
;
go

ALTER TABLE Projects
ADD CONSTRAINT FK_Projects_Teams FOREIGN KEY (TeamId) REFERENCES Teams(TeamId);


/***** Table No. 4 - Sprints ****/
CREATE TABLE Sprints
(
	SprintId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(100) NOT NULL,
	Description nvarchar(255) NOT NULL,
	StartDate DATE NOT NULL,
	ReleaseDate DATE NULL,
	Status nvarchar(50) NOT NULL,
	ProjectId int NOT NULL,
	CONSTRAINT PK_Sprints PRIMARY KEY CLUSTERED (SprintId ASC)
)
;
go

ALTER TABLE Sprints
ADD
	CONSTRAINT CK_Sprints_Status CHECK (Status IN ('Planning', 'Ongoing', 'Released')),
	CONSTRAINT FK_Sprints_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId)
;
go


/***** Table No. 5 - UserStories ****/
CREATE TABLE UserStories
(
	UserStoryId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(100) NOT NULL,
	Description nvarchar(255) NOT NULL,
	CreateDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	Point int NOT NULL,
	Photo VARBINARY(MAX) NULL,
	Status nvarchar(50) NOT NULL,
	OwnerId int NOT NULL,
	SprintId int NOT NULL,
	CONSTRAINT PK_UserStories PRIMARY KEY CLUSTERED (UserStoryId ASC)
)
;
go

ALTER TABLE UserStories
ADD
	CONSTRAINT CK_UserStories_Point CHECK (Point<101 AND Point>0),
	CONSTRAINT CK_UserStories_Status CHECK (Status IN ('Todo', 'Documenting', 'InValidation', 'Ready', 'DEV','TEST','DONE')),
	CONSTRAINT FK_UserStories_Users FOREIGN KEY (OwnerId) REFERENCES Users(UserId),
	CONSTRAINT FK_UserStories_Sprints FOREIGN KEY (SprintId) REFERENCES Sprints(SprintId)
;
go

/***** Table No. 6 - Issues ****/
CREATE TABLE Issues
(
	IssueId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(100) NOT NULL,
	Description nvarchar(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	Priority nvarchar(50) NOT NULL,
	Photo VARBINARY(MAX) NULL,
	Category nvarchar(50) NOT NULL,
	Status nvarchar(50) NOT NULL,
	OwnerId int NOT NULL,
	UserStoryId int NOT NULL,
	CONSTRAINT PK_Issues PRIMARY KEY CLUSTERED (IssueId ASC)
)
;
go

ALTER TABLE Issues
ADD
	CONSTRAINT CK_Issues_Priority CHECK (Priority IN ('VeryLow', 'Low', 'Medium', 'High', 'VeryHigh')), 
	CONSTRAINT CK_Issues_Category CHECK (Category IN ('Defect', 'Task')),
	CONSTRAINT CK_Issues_Status CHECK (Status IN ('Todo', 'InProcess', 'Blocked', 'Verified', 'Resolved')),
	CONSTRAINT FK_Issues_Users FOREIGN KEY (OwnerId) REFERENCES Users(UserId),
	CONSTRAINT FK_Issues_UserStories FOREIGN KEY (UserStoryId) REFERENCES UserStories(UserStoryId)
;
go


