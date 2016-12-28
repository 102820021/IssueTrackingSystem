﻿using IssueTrackingSystem.AMS.Controller;
using IssueTrackingSystem.ITS.Controller;
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
    public partial class IssueInfoView : IssueTrackingSystem.View.BaseView
    {
        private UserModel userModel;
        private IssueModel issueModel;
        private ProjectModel projectModel;
        private UserController userController;
        private IssueController issueController;
        private List<Issue> issueDetails;
        private List<User> projectMembers;
        private User reporter;
        private User assignee;

        public IssueInfoView()
        {
            InitializeComponent();
            userModel = new UserModel();
            issueModel = new IssueModel();
            projectModel = new ProjectModel();
            userController = new UserController(userModel);
            issueController = new IssueController(userModel, issueModel, projectModel);
        }

        public IssueInfoView(int issueId)
        {
            InitializeComponent();
            userModel = new UserModel();
            issueModel = new IssueModel();
            projectModel = new ProjectModel();
            userController = new UserController(userModel);
            issueController = new IssueController(userModel, issueModel, projectModel);

            issueDetails = issueController.getIssuedetails(issueId);
        }

        private void IssueInfoViewLoad(object sender, EventArgs e)
        {
            updateIssueInfoView();
        }

        private void submitButtonClicked(object sender, EventArgs e)
        {
            if (submitButton.Text.Equals("提交議題"))
            {
                submitButton.Text = "確認提交";
                enableEditIssueInfo(true);
            }
            else
            {
                enableEditIssueInfo(false);
                submitButton.Text = "提交議題";

                Issue issue = new Issue();
                issue.IssueName = issueNameLabel.Text;
                issue.Description = issueDescriptionRichTextBox.Text;
                issue.State = issueStateComboBox.SelectedText;
                issue.Priority = issuePriorityComboBox.SelectedText;
                issue.Serverity = issueSeverityComboBox.SelectedText;
                issue.PersonInChargeId = 1;
                issue.IssueId = issueController.updateIssue(issue);

                if (issue.IssueId != 0)
                {
                    issueDetails = issueController.getIssuedetails(issue.IssueId);
                    updateIssueInfoView();
                }
            }
        }

        private void enableEditIssueInfo(bool isEnabled)
        {
            issueStateComboBox.Enabled = isEnabled;
            issuePriorityComboBox.Enabled = isEnabled;
            issueSeverityComboBox.Enabled = isEnabled;
            issueAssigneeComboBox.Enabled = isEnabled;
            issueDescriptionRichTextBox.Enabled = isEnabled;
        }

        private void updateIssueInfoView()
        {
            reporter = userController.getUser(issueDetails[0].ReporterId);
            assignee = userController.getUser(issueDetails[0].PersonInChargeId);

            issueNameLabel.Text = issueDetails[0].IssueName;
            issueStateComboBox.Text = issueDetails[0].State;
            issuePriorityComboBox.Text = issueDetails[0].Priority;
            issueSeverityComboBox.Text = issueDetails[0].Serverity;
            issueReporterLabel.Text = reporter.UserName;
            issueReportDateLabel.Text = issueDetails[0].ReportDate.ToString();
            issueAssigneeComboBox.Text = assignee.UserName;
            issueDescriptionRichTextBox.Text = issueDetails[0].Description;
        }
    }
}
