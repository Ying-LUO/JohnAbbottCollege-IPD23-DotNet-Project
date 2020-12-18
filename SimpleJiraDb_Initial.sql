USE SimpleJiraDB;
GO

DROP TABLE Sprints;
DROP TABLE Issues;
DROP TABLE Users;
DROP TABLE Teams;
GO

/***** Table No. 1 - Teams ****/
CREATE TABLE Teams
(
	TeamId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	CONSTRAINT PK_Teams PRIMARY KEY CLUSTERED (TeamId ASC)
)
;
go

/***** Table No. 2 - Users ****/
CREATE TABLE Users
(
	UserId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(30) NOT NULL,
	Role nvarchar(20) NOT NULL,
	TeamId int NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (UserId ASC)
)
;
go

ALTER TABLE Users
ADD
	CONSTRAINT FK_Users_Teams FOREIGN KEY (TeamId) REFERENCES Teams(TeamId)
;
go

/***** Table No. 3 - Sprints ****/
CREATE TABLE Sprints
(
	SprintId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	Description nvarchar(200) NOT NULL,
	StartDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	Status nvarchar(20) NOT NULL,
	OwnerId int NOT NULL,
	CONSTRAINT PK_Sprints PRIMARY KEY CLUSTERED (SprintId ASC)
)
;
go

ALTER TABLE Sprints
ADD
	CONSTRAINT CK_Sprints_Status CHECK (Status IN ('Planning', 'Ongoing', 'Completed')),
	CONSTRAINT FK_Sprints_Teams FOREIGN KEY (OwnerId) REFERENCES Teams(TeamId)
;
go

/***** Table No. 4 - Issues ****/
CREATE TABLE Issues
(
	IssueId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	Description nvarchar(200) NOT NULL,
	StartDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	Photo VARBINARY(MAX) NULL,
	Category nvarchar(20) NOT NULL,
	Status nvarchar(20) NOT NULL,
	OwnerId int NOT NULL,
	CONSTRAINT PK_Issues PRIMARY KEY CLUSTERED (IssueId ASC)
)
;
go

ALTER TABLE Issues
ADD
	CONSTRAINT CK_Issues_Category CHECK (Status IN ('UserStory', 'Defect', 'Task', 'Backlog')),
	CONSTRAINT CK_Issues_Status CHECK (Status IN ('Todo', 'InProcess', 'UnderReview', 'Done')),
	CONSTRAINT FK_Issues_Users FOREIGN KEY (OwnerId) REFERENCES Users(UserId)
;
go


