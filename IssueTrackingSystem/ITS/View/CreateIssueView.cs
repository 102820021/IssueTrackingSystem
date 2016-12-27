﻿using IssueTrackingSystem.ITS.Controller;
using IssueTrackingSystem.Model;
using IssueTrackingSystem.Model.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IssueTrackingSystem.ITS.View
{
    public partial class CreateIssueView : IssueTrackingSystem.View.BaseView
    {
        private IssueModel issueModel;
        private UserModel userModel;
        private ProjectModel projectModel;
        private IssueController issueController;


        public CreateIssueView()
        {
            InitializeComponent();
            issueModel = new IssueModel();
            userModel = new UserModel();
            projectModel = new ProjectModel();
            issueController = new IssueController(userModel, issueModel, projectModel);
        }

        private void submitButtonClicked(object sender, EventArgs e)
        {
            Issue issue = new Issue();
            issue.ProjectId = 4;
            issue.IssueName = issueNameTextBox.Text;
            issue.Priority = int.Parse(issuePriorityComboBox.Text);
            issue.Serverity = issueSeverityComboBox.Text;
            issue.Description = issueDescriptionRichTextBox.Text;
            issue.PersonInChargeId = 1;
            issue.State = "待審核";
            issue = issueController.createIssue(issue);

            this.Owner.Show();
            this.Close();
        }
    }
}
