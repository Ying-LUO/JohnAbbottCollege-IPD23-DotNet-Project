INSERT INTO Projects(Name)
VALUES ('Test Project Another');

INSERT INTO Teams(Name)
VALUES ('Test Team');

INSERT INTO Users(Name, Role, TeamId)
VALUES ('James', 'Tester', 2);

INSERT INTO Sprints(Name, Description, StartDate, Status, ProjectId, OwnerId)
VALUES ('Sprint 2020 04', 'Test Sprint for 2020 December', '2020-12-20', 'Ongoing', 2, 2);

INSERT INTO UserStories(Name, Description, CreateDate, Point, Status, SprintId, OwnerId)
VALUES ('New requirement', 'As a tester of this application, I need a system design for review', '2020-12-21', 80, 'TEST', 2, 2);

INSERT INTO Issues(Name, Description, StartDate, Priority, Category, Status, UserStoryId, SprintId, OwnerId)
VALUES ('login failed', 'when I try to login, it throw exception', '2020-12-21', 'Medium', 'Defect', 'Resolved', 1, 1, 1);

INSERT INTO Issues(Name, Description, StartDate, Priority, Category, Status, UserStoryId, SprintId, OwnerId)
VALUES ('defect fix', 'Fix the exception thrown when login', '2020-12-21', 'VeryHigh', 'Task', 'Blocked', 1, 1, 1);