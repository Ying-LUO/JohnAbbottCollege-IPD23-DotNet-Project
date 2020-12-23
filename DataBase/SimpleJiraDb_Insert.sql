INSERT INTO Projects(Name)
VALUES ('New Project'),
		('Test Project Another');

INSERT INTO Teams(Name)
VALUES ('Development Team'),
		('Test Team');

INSERT INTO Users(Name, Role, TeamId)
VALUES ('David', 'Developer', 1),
		('James', 'Tester', 2);

INSERT INTO Sprints(Name, Description, StartDate, Status, ProjectId, OwnerId)
VALUES ('Sprint 2020 04', 'Test Sprint for 2020 December', '2020-12-20', 'Ongoing', 1, 1),
		('Sprint 2019', 'Relase Sprint for 2019 winter', '2019-09-12', 'Released', 2, 2),
		('Sprint 2020 01', 'Relase Sprint for 2020 Summer', '2020-02-18', 'Planning', 1, 2);

INSERT INTO UserStories(Name, Description, CreateDate, Point, Status, SprintId, OwnerId)
VALUES ('New requirement', 'As a tester of this application, I need a system design for review', '2020-12-21', 80, 'TEST', 3, 1);

INSERT INTO Issues(Name, Description, StartDate, Priority, Category, Status, UserStoryId, SprintId, OwnerId)
VALUES ('login failed', 'when I try to login, it throw exception', '2020-12-21', 'Medium', 'Defect', 'Resolved', 1, 3, 1);

INSERT INTO Issues(Name, Description, StartDate, Priority, Category, Status, UserStoryId, SprintId, OwnerId)
VALUES ('defect fix', 'Fix the exception thrown when login', '2020-12-21', 'VeryHigh', 'Task', 'Blocked', 1, 3, 1);