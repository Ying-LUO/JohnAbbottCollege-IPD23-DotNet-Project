﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleJiraProject
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SimpleJiraDBEntities : DbContext
    {
        public SimpleJiraDBEntities()
            : base("name=SimpleJiraDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserStory> UserStories { get; set; }
    }
}
