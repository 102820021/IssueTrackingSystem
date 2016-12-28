﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IssueTrackingSystem.Model.ApiModel;
using IssueTrackingSystem.Model.DataModel;
using IssueTrackingSystem.Model;

namespace IssueTrackingSystem.PMS.View
{
    public partial class CreateUpdateProject : IssueTrackingSystem.View.BaseView
    {
        public CreateUpdateProject(String purpose)
        {
            InitializeComponent();
            _createUpdate.Text = purpose;
        }

        private void ClickCreateUpdate(object sender, EventArgs e)
        {
            User user = SecurityModel.getInstance().AuthenticatedUser;
            ProjectModel projectModel = new ProjectModel();
            Project project = new Project();
            project.ProjectName = _projectNameInput.Text;
            project.Description = _descriptionInput.Text;
            projectModel.createProject(user.UserId, project);
        }
    }
}
