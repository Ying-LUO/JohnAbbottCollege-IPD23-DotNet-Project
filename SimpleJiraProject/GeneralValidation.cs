using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    class GeneralValidation
    {
        public bool LoginName_Check(string loginName)
        {
            List<string> NameList = Globals.simpleJiraDB.Users.AsEnumerable().Select(u => u.LoginName).ToList<string>();
            return NameList.Contains(loginName);
        }

        public bool IsValidEmail(string email)
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

        public bool IsValidPassword(string pwd)
        {
            return pwd.Length < 13 && pwd.Length > 7;
        }

        public bool Password_Check(string pwd, string confirmPwd)
        {
            return pwd == confirmPwd;
        }

        public int Team_Check(string team)
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

        public bool IsValidName(string name)
        {
            return name.Length < 30 && name.Length > 1;
        }

        public bool IsValidDate(DateTime date1, DateTime date2)
        {
            return date1 <= date2;
        }

        public bool IsValidPoint(int point)
        {
                return point > 0 && point < 101;
        }

    }
}
