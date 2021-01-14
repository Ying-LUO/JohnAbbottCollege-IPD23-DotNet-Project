using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    public class UserValidation
    {
        public static bool LoginName_Check(string loginName)
        {
            List<string> NameList = Globals.simpleJiraDB.Users.AsEnumerable().Select(u => u.LoginName).ToList<string>();
            return NameList.Contains(loginName);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(string pwd)
        {
            return pwd.Length < 13 && pwd.Length > 7;
        }

        public static bool Password_Check(string pwd, string confirmPwd)
        {
            return pwd == confirmPwd;
        }

        public static int Team_Check(string team)
        {
            Team chooseTeam = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(team)).FirstOrDefault();
            int teamId = chooseTeam != null ? chooseTeam.TeamId : 0;
            if (teamId == 0)
            {
                Team newTeam = new Team { Name = team };
                Globals.simpleJiraDB.Teams.Add(newTeam);
                Globals.simpleJiraDB.SaveChanges();
                return newTeam.TeamId;
            }
            else
            {
                return chooseTeam.TeamId;
            }
        }
    }
}
