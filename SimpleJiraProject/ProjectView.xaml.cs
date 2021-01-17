﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : UserControl
    {
        Project project;
        string updatedTeamName;
        string projectName;
        
        public ProjectView()
        {
            InitializeComponent();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!Globals.Validator.IsValidShortName(tbProjectName.Text))
            {
                new MessageBoxCustom("Project Name must be between 2-30 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                tbProjectName.Text = projectName;
                tbEditProjectName.Text = projectName;
                project.Name = projectName;
                Globals.simpleJiraDB.SaveChanges();
                return;
            }
            else
            {
                project.Name = tbEditProjectName.Text;
                updatedTeamName = (string)comboTeam.SelectedItem;
                project.TeamId = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(updatedTeamName)).Select(t => t.TeamId).FirstOrDefault();
                Globals.simpleJiraDB.SaveChanges();
                Globals.currentTeamProjectList = Globals.simpleJiraDB.Projects.Include("Team").ToList();
                Globals.AppWindow.LoadDataFromDb(Globals.currentUser);
            }
               
            
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            foreach (Project p in Globals.currentTeamProjectList)
            {
                if(p.Name == tbProjectName.Text)
                {
                    project = p;
                    projectName = p.Name;
                    comboTeam.SelectedItem = p.Team.Name;
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Project SelectedProject = Globals.simpleJiraDB.Projects.Where(p => p.Name.Equals(tbProjectName.Text)).FirstOrDefault();
            Globals.simpleJiraDB.Projects.Remove(SelectedProject);
            Globals.simpleJiraDB.SaveChanges();
            Globals.AppWindow.LoadDataFromDb(Globals.currentUser);
        }
    }
    }
