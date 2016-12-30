﻿using IssueTrackingSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IssueTrackingSystem.PMS.Controller;
using IssueTrackingSystem.Model.DataModel;
using IssueTrackingSystem.Model.ApiModel;

namespace IssueTrackingSystem.PMS.View
{
    public partial class Member : IssueTrackingSystem.View.BaseView
    {
        private UserModel userModel;
        private IssueModel issueModel;
        private ProjectModel projectModel;
        private ProjectMemberModel projectMemberModel;
        private ProjectMemberController memberController;
        private Project project = new Project();
        private String UPDATE = "Update";
        private String DELETE = "Delete";

        public Member(UserModel userModel, IssueModel issueModel, ProjectModel projectModel, Project project, ProjectMemberModel projectMemberModel)
                    : base(userModel, issueModel, projectModel, projectMemberModel)
        {
            InitializeComponent();
            this.userModel = userModel;
            this.issueModel = issueModel;
            this.projectModel = projectModel;
            this.project = project;
            this.projectMemberModel = projectMemberModel;
            memberController = new ProjectMemberController(projectMemberModel, userModel);
            _projectName.Text = project.ProjectName;
            this.projectModel.projectDataChanged += UpdateView;
            ShowData();
//            _dataGridView.Rows.Add("1", "TEST", "123", "general_user", "Update", "Delete");
        }

        public void ShowData()
        {
            if (_memberList.SelectedTab.Name.Equals("_existingMemberTable"))
            {
                ShowgMember(_dataGridView, true);
            }
            else if (_memberList.SelectedTab.Name.Equals("_joiningMemberTable"))
            {
                ShowgMember(_dataGridViewJoining, false);
            }
            else if (_memberList.SelectedTab.Name.Equals("_inviteMemberTable"))
            {

            }
        }

        public void ShowgMember(DataGridView table, bool joined)
        {
            List<ProjectMember> memberList = new List<ProjectMember>();
            List<User> userList = new List<User>();
            memberList = memberController.getMemberByProjectId(project.ProjectId, joined);
            userList = memberController.getUserByProjectId(project.ProjectId, joined);
            for(int i = 0; i < userList.Count; i++)
            {
                table.Rows.Add(userList[i].UserId.ToString(), userList[i].UserName, userList[i].EmailAddress, memberList[i].Role, UPDATE, DELETE);
            }
        }

        private void InviteMemberClicked(object sender, EventArgs e)
        {
            UserApiModel member = new UserApiModel();
            member = userModel.getUserInfoByName(_nameInput.Text);
            
        }

        private void MemberListSelectedIndexChanged(object sender, EventArgs e)
        {
            ShowData();
        }

        private void DataGridViewCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            if(e.RowIndex >= 0)
            {
                if(col == 4)
                {
                    UpdateMember(new ProjectMember(Convert.ToInt16(_dataGridView.Rows[row].Cells[0].Value), project.ProjectId, _dataGridView.Rows[row].Cells[3].Value.ToString()), true);
                }
                if(col == 5)
                {
                    DeleteMember(new ProjectMember(Convert.ToInt16(_dataGridView.Rows[row].Cells[0].Value), project.ProjectId, _dataGridView.Rows[row].Cells[3].Value.ToString()));
                }
            }
        }

        public void UpdateMember(ProjectMember member, bool joined)
        {
            memberController.updateInfo(member, joined);
        }

        public void DeleteMember(ProjectMember member)
        {
            memberController.deleteMember(SecurityModel.getInstance().AuthenticatedUser.UserId, member);
        }

        private void DataGridViewJoiningCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                if (col == 4)
                {
                    UpdateMember(new ProjectMember(Convert.ToInt16(_dataGridViewJoining.Rows[row].Cells[0].Value), project.ProjectId, _dataGridView.Rows[row].Cells[3].Value.ToString()), false);
                }
                if (col == 5)
                {
                    DeleteMember(new ProjectMember(Convert.ToInt16(_dataGridViewJoining.Rows[row].Cells[0].Value), project.ProjectId, _dataGridView.Rows[row].Cells[3].Value.ToString()));
                }
            }
        }

        public void UpdateView()
        {
            project = projectModel.getProjectInfo(SecurityModel.getInstance().AuthenticatedUser.UserId, project.ProjectId);
            _projectName.Text = project.ProjectName;
        }
    }
}
