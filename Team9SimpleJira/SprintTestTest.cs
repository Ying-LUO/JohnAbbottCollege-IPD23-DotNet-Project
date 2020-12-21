using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team9SimpleJira
{
    class SprintTestTest
    {
        public string Name { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Status { get; set; }

        public string Description { get; set; }

        public SprintTestTest(string name, string date1, string date2, string status, string description)
        {
            Name = name;
            Date1 = date1;
            Date2 = date2;
            Status = status;
            Description = description;
        }

    }

    
}
