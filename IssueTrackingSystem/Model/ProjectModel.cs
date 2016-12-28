﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTrackingSystem.Model.DataModel;
using Newtonsoft.Json;
using IssueTrackingSystem.Model.ApiModel;
using System.Net;
using System.IO;
using Chsword;

namespace IssueTrackingSystem.Model
{
    class ProjectModel
    {
        public int createProject(int userId, Project project)
        {
            int state;
            var req = WebRequest.Create(Server.ApiUrl + "/projects/" + userId);
            req.Method = "POST";
            req.ContentType = "application/json";
            String contentData = "{\"projectName\":\"" + project.ProjectName + "\"," +
                                  "\"description\":\"" + project.Description + "\"}";
            using (var writer = new StreamWriter(req.GetRequestStream()))
            {
                writer.Write(contentData);
            }

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                state = projectApiModel.state;
            }
            return state;
        }

        public Project getProjectInfo(int userId, int projectId)
        {
            Project project = null;
            var req = WebRequest.Create(Server.ApiUrl + "/projects/" + userId + "/" + projectId);
            req.Method = "GET";

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                if(projectApiModel.state == 0)
                {
                    project = new Project();
                    project.ProjectId = projectApiModel.projectId;
                    project.ProjectName = projectApiModel.projectName;
                    project.Description = projectApiModel.description;
                    project.Manager = projectApiModel.manager;
                    project.TimeStamp = DateTime.FromFileTime(long.Parse((string)projectApiModel.timeStamp));
                }
                else
                {
                    project = null;
                }
            }
            return project;
        }

        public List<Project> getProjectListByUserId(int userId)
        {
            List<Project> projectList = new List<Project>();
            var req = WebRequest.Create(Server.ApiUrl + "/projects/list/" + userId);
            req.Method = "GET";

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                if (projectApiModel.state == 0)
                {
                    foreach (dynamic o in projectApiModel.list)
                    {
                        Project project = new Project();
                        project.ProjectId = o.projectId;
                        project.ProjectName = o.projectName;
                        project.Description = o.description;
                        project.Manager = o.manager;
                        project.TimeStamp = DateTime.FromFileTime(long.Parse((string)o.timeStamp));
                        projectList.Add(project);
                    }
                }
            }
            return projectList;
        }

        public List<Project> getInvitedProjectListByUserId(int userId)
        {
            List<Project> projectList = new List<Project>();
            var req = WebRequest.Create(Server.ApiUrl + "/projects/" + userId);
            req.Method = "GET";

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                if (projectApiModel.state == 0)
                {
                    foreach (dynamic o in projectApiModel.list)
                    {
                        Project project = new Project();
                        project.ProjectId = o.projectId;
                        project.ProjectName = o.projectName;
                        project.Description = o.description;
                        project.Manager = o.manager;
                        project.TimeStamp = DateTime.FromFileTime(long.Parse((string)projectApiModel.timeStamp));
                        projectList.Add(project);
                    }
                }
            }
            return projectList;
        }

        public List<Project> getAllProjectList(int userId)
        {
            List<Project> projectList = null;
            var req = WebRequest.Create(Server.ApiUrl + "/all-projects/" + userId);
            req.Method = "GET";

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                if (projectApiModel.state == 0)
                {
                    foreach (dynamic o in projectApiModel.list)
                    {
                        Project project = new Project();
                        project.ProjectId = o.projectId;
                        project.ProjectName = o.projectName;
                        project.Description = o.description;
                        project.Manager = o.manager;
                        project.TimeStamp = DateTime.FromFileTime(long.Parse((string)projectApiModel.timeStamp));
                        projectList.Add(project);
                    }
                }
            }
            return projectList;
        }

        public int updateProjectInfo(int userId, Project project)
        {
            int state = 0;
            var req = WebRequest.Create(Server.ApiUrl + "/projects/" + userId + "/" + project.ProjectId);
            req.Method = "PUT";
            req.ContentType = "application/json";
            String contentData = "{\"projectName\":\"" + project.ProjectName + "\"," +
                                  "\"description\":\"" + project.Description + "\"}";
            using (var writer = new StreamWriter(req.GetRequestStream()))
            {
                writer.Write(contentData);
            }

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                state = projectApiModel.state;
            }
            return state;
        }

        public void deleteProject()
        {

        }
    }
}
