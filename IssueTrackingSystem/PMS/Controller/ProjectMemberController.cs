﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTrackingSystem.Model;
using IssueTrackingSystem.Model.DataModel;

namespace IssueTrackingSystem.PMS.Controller
{
    class ProjectMemberController
    {
        ProjectMemberModel model = new ProjectMemberModel();

        public int createMember(ProjectMember member)
        {
            return model.createMember(member);
        }

        public List<ProjectMember> getMemberByProjectId(int projectId, bool joined)
        {
            return model.getMemberByProjectId(projectId, joined);
        }

        public int updateInfo(ProjectMember member, bool joined)
        {
            return model.updateInfo(member, joined);
        }

        public int deleteMember(int managerId, ProjectMember member)
        {
            return model.deleteMember(managerId, member);
        }
    }
}
