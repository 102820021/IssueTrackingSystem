﻿using IssueTrackingSystem.Model;
using IssueTrackingSystem.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackingSystem.ITS.Controller
{
    class IssueController
    {
        private User user;
        private UserModel userModel;
        private ProjectModel projectModel;
        private IssueModel issueModel;
        private List<Issue> issueList;

        public IssueController(UserModel userModel, IssueModel issueModel, ProjectModel projectModel)
        {
            this.userModel = userModel;
            this.issueModel = issueModel;
            this.projectModel = projectModel;
            user = SecurityModel.getInstance().AuthenticatedUser;
            issueList = new List<Issue>();

            getIssueList();
        }

        public List<Issue> listAllIssues() {
            issueList = issueModel.getAllIssueList();

            return issueList;
        }
        
        public List<Issue> listIssues(String keyword, int searchType)
        {
            List<Issue> searchedIssueList = new List<Issue>();

            getIssueList();

            switch (searchType) {
                case (int)Issue.SearchType.ByIssueName:
                    foreach (Issue issue in issueList)
                    {
                        if (issue.IssueName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            searchedIssueList.Add(issue);
                        }
                    }
                    break;
                case (int)Issue.SearchType.ByProjectName:
                    foreach (Issue issue in issueList)
                    {
                        //Project project = projectModel.getProjectInfo(issue.ProjectId);
                        if (issue.IssueName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            searchedIssueList.Add(issue);
                        }
                    }
                    break;
                case (int)Issue.SearchType.ByReporterName:
                    foreach (Issue issue in issueList) { 
                        User user = userModel.getUserInfo(issue.ReporterId);
                        if (user.UserName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1) {
                            searchedIssueList.Add(issue);
                        }
                    }
                    break;
                case (int)Issue.SearchType.ByPersonInChargeName:
                    foreach (Issue issue in issueList)
                    {
                        User user = userModel.getUserInfo(issue.PersonInChargeId);
                        if (user.UserName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            searchedIssueList.Add(issue);
                        }
                    }
                    break;
            }

            return searchedIssueList;
        }

        public List<Issue> getIssuedetails(int issueId)
        {
            List<Issue> historyIssueList = new List<Issue>();
            Issue issue = new Issue();

            issue = issueModel.getIssueInfo(issueId);
            historyIssueList.Add(issue);
            getIssueList();
            foreach (Issue historyIssue in issueList) {
                if (historyIssue.IssueGroupId == issue.IssueGroupId) {
                    historyIssueList.Add(historyIssue);
                }
            }

            return historyIssueList;
        }

        public Issue createIssue(Issue issue)
        {
            issue = issueModel.createIssue(issue);

            return issue;
        }

        public Issue updateIssue(Issue issue)
        {

            //api

            return issue;
        }

        private void getIssueList()
        {

            if (user.Authority == (int)User.AuthorityEnum.GeneralUser)
            {
                foreach (Project project in user.JoinedProjects)
                {
                    List<Issue> newIssueList = new List<Issue>();
                    newIssueList = issueModel.getIssueListByProjectId(project.ProjectId);
                    foreach (Issue issue in newIssueList)
                    {
                        if (issue.FinishDate == DateTime.MaxValue)
                            issueList.Add(issue);
                    }
                }
            }
            else
            {
                issueList = listAllIssues();
            }
        }
    }
}
