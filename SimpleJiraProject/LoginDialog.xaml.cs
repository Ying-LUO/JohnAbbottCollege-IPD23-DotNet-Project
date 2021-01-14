﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        User loginUser;
        public event Action<User> LoginCallback;

        public LoginDialog()
        {
            InitializeComponent();
            try
            {
                Globals.simpleJiraDB = new SimpleJiraDBEntities();
            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Fatal error connecting to database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                Environment.Exit(1);
            }
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginUser = Globals.simpleJiraDB.Users.Where(u => u.LoginName == tbLoginName.Text).FirstOrDefault();

                if (loginUser != null)
                {
                    string pwd = SecurePassword.Decrypt(loginUser.PWDEncrypted);
                    if (tbPassword.Password.Equals(pwd))
                    {
                        this.DialogResult = true;
                        LoginCallback?.Invoke(loginUser);
                        this.Close();
                    }
                    else
                    {
                        new MessageBoxCustom("Password Incorrect", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    }
                }
                else
                {
                    new MessageBoxCustom("User not exist", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Login into system:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void btSignupFromLogin_Click(object sender, RoutedEventArgs e)
        {
            SignupDialog signup = new SignupDialog();
            signup.Owner = this;
            signup.SignupCallback += (u) => { loginUser = u; };
            bool? result = signup.ShowDialog();  // this line must be stay after the assignment, otherwise value is not assigned
            if (loginUser!= null)
            {
                tbLoginName.Text = loginUser.LoginName;
            }
        }
    }
}
