using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    public partial class Project
    {
        [NotMapped]
        public ICollection<string> AllTeamNamesList { get; set; }
    }

    public partial class Sprint
    {
        [NotMapped]
        public SprintStatusEnum SprintStatus { get; set; }
    }

    public enum SprintStatusEnum { Planning, Ongoing, Released }

    public enum IssuePriorityEnum { VeryLow, Low, Medium, High, VeryHigh}

    public enum IssueStatusEnum { Todo, InProcess, Blocked, Verified, Resolved}

    public enum IssueCategoryEnum { Defect, Task}
}
