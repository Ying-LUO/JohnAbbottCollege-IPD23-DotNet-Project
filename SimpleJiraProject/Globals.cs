using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    class Globals
    {
        public static SimpleJiraDBEntities simpleJiraDB;
        public static List<Project> currentTeamProjectList;
        public static List<User> currentTeamUserList;
        public static List<Sprint> currentSprintList;
        public static List<UserStory> currentUserStoryList;
        public static MainWindow AppWindow;
        public static User currentUser;
        public static Project SelectedProject;
        public static Sprint SelectedSprint;
        public static UserStory SelectedUserStory;
    }
}
