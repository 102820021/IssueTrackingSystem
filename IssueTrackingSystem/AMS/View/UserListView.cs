﻿using IssueTrackingSystem.AMS.Controller;
using IssueTrackingSystem.Model;
using IssueTrackingSystem.Model.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IssueTrackingSystem.AMS.View
{
    public partial class UserListView : IssueTrackingSystem.View.BaseView
    {
        private UserModel userModel;
        private IssueModel issueModel;
        private ProjectModel projectModel;
        private ProjectMemberModel projectMemberModel;
        private SecurityController securityController;
        private UserController userController;
        private List<User> userList;

        public UserListView(UserModel userModel, IssueModel issueModel, ProjectModel projectModel, ProjectMemberModel projectMemberModel)
            : base(userModel, issueModel, projectModel, projectMemberModel)
        {
            InitializeComponent();
            this.userModel = userModel;
            this.issueModel = issueModel;
            this.projectModel = projectModel;
            this.projectMemberModel = projectMemberModel;
            userModel.userDataChanged += updateUserList;
            issueModel.issueDataChanged += updateUserList;
            projectModel.projectDataChanged += updateUserList;
            securityController = new SecurityController(userModel);
            userController = new UserController(userModel);
        }

        private void AccountListViewLoad(object sender, EventArgs e)
        {
            updateUserList();
        }

        private void userRoleValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0 && e.RowIndex < userListDataGridView.RowCount)
            {
                User user = userModel.getUserInfo((int)userListDataGridView.Rows[e.RowIndex].Cells[0].Value);
                if ((String)userListDataGridView.Rows[e.RowIndex].Cells[3].Value == "系統管理員")
                    user.Authority = (int)User.AuthorityEnum.SystemManager;
                else
                    user.Authority = (int)User.AuthorityEnum.GeneralUser;
                int state = userController.updateUser(user);
                switch (state) { 
                    case (int)ErrorManager.ErrorCode.CannotUpdateProjectManagerInfo:
                        statusStrip1.Text = "無法修改專案管理員資料";
                        break;
                    case (int)ErrorManager.ErrorCode.InvalidAuthoritySetting:
                        statusStrip1.Text = "不合法的權限設定";
                        break;
                    case (int)ErrorManager.ErrorCode.Success:
                        statusStrip1.Text = "已成功修改權限";
                        break;
                }
            }
        }

        private void userListDataGridViewCellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex >= 0 && e.RowIndex < userListDataGridView.RowCount)
            {
                int state = 0;
                state = userController.deleteUser((int)userListDataGridView.Rows[e.RowIndex].Cells[0].Value);
                switch (state)
                {
                    case (int)ErrorManager.ErrorCode.CannotUpdateProjectManagerInfo:
                        statusStrip1.Text = "無法刪除專案管理員";
                        break;
                    case (int)ErrorManager.ErrorCode.InvalidAuthoritySetting:
                        statusStrip1.Text = "不合法的刪除動作";
                        break;
                    case (int)ErrorManager.ErrorCode.Success:
                        statusStrip1.Text = "已成功刪除使用者";
                        break;
                }
            }
        }

        private void updateUserList(){
            statusStrip1.Text = "";
            userList = securityController.listAccounts();
            userListDataGridView.Rows.Clear();
            foreach (User user in userList)
            {
                String userRole = null;
                if (user.Authority == (int)User.AuthorityEnum.GeneralUser)
                    userRole = "一般使用者";
                else
                    userRole = "系統管理員";
                userListDataGridView.Rows.Add(new Object[] { user.UserId, user.UserName, user.EmailAddress, userRole, user.JoinedProjects.Count, user.InvitedProjects.Count, user.Issues.Count, "刪除" });
            }
        }
    }
}
