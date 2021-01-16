using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    public class IssueListView
    {
        public int IssueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public int UserStoryId { get; set; }

        public virtual User User { get; set; }
        public virtual UserStory UserStory { get; set; }

    }
}
