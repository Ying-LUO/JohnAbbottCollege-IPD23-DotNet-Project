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
}
