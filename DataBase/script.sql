/****** Object:  Table [dbo].[Issues]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Issues](
	[IssueId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[CompleteDate] [datetime] NULL,
	[Priority] [nvarchar](50) NOT NULL,
	[Photo] [varbinary](max) NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[UserStoryId] [int] NOT NULL,
 CONSTRAINT [PK_Issues] PRIMARY KEY CLUSTERED 
(
	[IssueId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TeamId] [int] NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sprints]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sprints](
	[SprintId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[StartDate] [date] NOT NULL,
	[ReleaseDate] [date] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_Sprints] PRIMARY KEY CLUSTERED 
(
	[SprintId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sysdiagrams]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysdiagrams](
	[name] [sysname] NOT NULL,
	[principal_id] [int] NOT NULL,
	[diagram_id] [int] IDENTITY(1,1) NOT NULL,
	[version] [int] NULL,
	[definition] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[diagram_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_principal_name] UNIQUE NONCLUSTERED 
(
	[principal_id] ASC,
	[name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teams]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[TeamId] [int] NOT NULL,
	[PWDEncrypted] [nvarchar](200) NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EMAIL] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoginName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserStories]    Script Date: 2021-01-18 8:28:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserStories](
	[UserStoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CompleteDate] [datetime] NULL,
	[Point] [int] NOT NULL,
	[Photo] [varbinary](max) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[OwnerId] [int] NOT NULL,
	[SprintId] [int] NOT NULL,
 CONSTRAINT [PK_UserStories] PRIMARY KEY CLUSTERED 
(
	[UserStoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_Issues_Users] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_Issues_Users]
GO
ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_Issues_UserStories] FOREIGN KEY([UserStoryId])
REFERENCES [dbo].[UserStories] ([UserStoryId])
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_Issues_UserStories]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([TeamId])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Teams]
GO
ALTER TABLE [dbo].[Sprints]  WITH CHECK ADD  CONSTRAINT [FK_Sprints_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
GO
ALTER TABLE [dbo].[Sprints] CHECK CONSTRAINT [FK_Sprints_Projects]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([TeamId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Teams]
GO
ALTER TABLE [dbo].[UserStories]  WITH CHECK ADD  CONSTRAINT [FK_UserStories_Sprints] FOREIGN KEY([SprintId])
REFERENCES [dbo].[Sprints] ([SprintId])
GO
ALTER TABLE [dbo].[UserStories] CHECK CONSTRAINT [FK_UserStories_Sprints]
GO
ALTER TABLE [dbo].[UserStories]  WITH CHECK ADD  CONSTRAINT [FK_UserStories_Users] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserStories] CHECK CONSTRAINT [FK_UserStories_Users]
GO
ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [CK_Issues_Category] CHECK  (([Category]='Task' OR [Category]='Defect'))
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [CK_Issues_Category]
GO
ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [CK_Issues_Priority] CHECK  (([Priority]='VeryHigh' OR [Priority]='High' OR [Priority]='Medium' OR [Priority]='Low' OR [Priority]='VeryLow'))
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [CK_Issues_Priority]
GO
ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [CK_Issues_Status] CHECK  (([Status]='Resolved' OR [Status]='Verified' OR [Status]='Blocked' OR [Status]='InProcess' OR [Status]='Todo'))
GO
ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [CK_Issues_Status]
GO
ALTER TABLE [dbo].[Sprints]  WITH CHECK ADD  CONSTRAINT [CK_Sprints_Status] CHECK  (([Status]='Released' OR [Status]='Ongoing' OR [Status]='Planning'))
GO
ALTER TABLE [dbo].[Sprints] CHECK CONSTRAINT [CK_Sprints_Status]
GO
ALTER TABLE [dbo].[UserStories]  WITH CHECK ADD  CONSTRAINT [CK_UserStories_Point] CHECK  (([Point]<(100) AND [Point]>(0)))
GO
ALTER TABLE [dbo].[UserStories] CHECK CONSTRAINT [CK_UserStories_Point]
GO
ALTER TABLE [dbo].[UserStories]  WITH CHECK ADD  CONSTRAINT [CK_UserStories_Status] CHECK  (([Status]='DONE' OR [Status]='TEST' OR [Status]='DEV' OR [Status]='Ready' OR [Status]='InValidation' OR [Status]='Documenting' OR [Status]='Todo'))
GO
ALTER TABLE [dbo].[UserStories] CHECK CONSTRAINT [CK_UserStories_Status]
GO
EXEC sys.sp_addextendedproperty @name=N'microsoft_database_tools_support', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sysdiagrams'
GO
