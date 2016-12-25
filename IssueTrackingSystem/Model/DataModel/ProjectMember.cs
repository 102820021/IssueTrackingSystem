﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackingSystem.Model.DataModel
{
    class ProjectMember
    {
        private int userId;
        private int projectId;
        private String role;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        public String Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}
