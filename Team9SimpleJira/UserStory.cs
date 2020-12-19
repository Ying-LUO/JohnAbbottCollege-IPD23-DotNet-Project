//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Team9SimpleJira
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserStory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserStory()
        {
            this.Issues = new HashSet<Issue>();
        }
    
        public int UserStoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public int Point { get; set; }
        public byte[] Photo { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public int SprintId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual User User { get; set; }
    }
}
