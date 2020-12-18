USE SimpleJiraDB;
GO

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

/***** Table No. 3 - Projects ****/
CREATE TABLE Projects
(
	ProjectId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	CONSTRAINT PK_Projects PRIMARY KEY CLUSTERED (ProjectId ASC)
)
;
go

/***** Table No. 4 - Sprints ****/
CREATE TABLE Sprints
(
	SprintId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	Description nvarchar(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	ReleaseDate DATETIME NULL,
	Status nvarchar(20) NOT NULL,
	ProjectId int NOT NULL,
	OwnerId int NOT NULL,
	CONSTRAINT PK_Sprints PRIMARY KEY CLUSTERED (SprintId ASC)
)
;
go

ALTER TABLE Sprints
ADD
	CONSTRAINT CK_Sprints_Status CHECK (Status IN ('Planning', 'Ongoing', 'Released')),
	CONSTRAINT FK_Sprints_Teams FOREIGN KEY (OwnerId) REFERENCES Teams(TeamId),
	CONSTRAINT FK_Sprints_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId)
;
go


/***** Table No. 5 - UserStories ****/
CREATE TABLE UserStories
(
	UserStoryId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	Description nvarchar(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	Point int NOT NULL,
	Photo VARBINARY(MAX) NULL,
	Status nvarchar(20) NOT NULL,
	OwnerId int NOT NULL,
	CONSTRAINT PK_UserStories PRIMARY KEY CLUSTERED (UserStoryId ASC)
)
;
go

ALTER TABLE UserStories
ADD
	CONSTRAINT CK_UserStories_Point CHECK (Point<100 AND Point>0),
	CONSTRAINT CK_UserStories_Status CHECK (Status IN ('Todo', 'Documenting', 'Pending for Validation', 'Ready', 'DEV','TEST','DONE')),
	CONSTRAINT FK_UserStories_Users FOREIGN KEY (OwnerId) REFERENCES Users(UserId)
;
go

/***** Table No. 6 - Issues ****/
CREATE TABLE Issues
(
	IssueId int IDENTITY(1,1) NOT NULL, -- auto-generated number
	Name nvarchar(20) NOT NULL,
	Description nvarchar(200) NOT NULL,
	StartDate DATETIME NOT NULL,
	CompleteDate DATETIME NULL,
	Priority int NOT NULL,
	Photo VARBINARY(MAX) NULL,
	Category nvarchar(20) NOT NULL,
	Status nvarchar(20) NOT NULL,
	OwnerId int NOT NULL,
	UserStoryId int NOT NULL,
	CONSTRAINT PK_Issues PRIMARY KEY CLUSTERED (IssueId ASC)
)
;
go

ALTER TABLE Issues
ADD
	CONSTRAINT CK_Issues_Priority CHECK (Priority<6 AND Priority >0),
	CONSTRAINT CK_Issues_Category CHECK (Status IN ('UserStory', 'Defect', 'Task', 'Backlog')),
	CONSTRAINT CK_Issues_Status CHECK (Status IN ('Todo', 'InProcess', 'Blocked', 'UnderReview', 'UnderVerification', 'Verified', 'Resolved')),
	CONSTRAINT FK_Issues_Users FOREIGN KEY (OwnerId) REFERENCES Users(UserId),
	CONSTRAINT FK_Issues_UserStoryId FOREIGN KEY (UserStoryId) REFERENCES UserStories(UserStoryId)
;
go


