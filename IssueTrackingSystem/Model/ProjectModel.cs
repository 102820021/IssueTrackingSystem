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
    public class ProjectModel
    {
        public event ModelChangedEventHandler projectDataChanged;
        public delegate void ModelChangedEventHandler();

        public ProjectApiModel createProject(int userId, Project project)
        {
            ProjectApiModel model = new ProjectApiModel();
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
                model.State = projectApiModel.state;
                model.ProjectContext.ProjectId = projectApiModel.project.projectId;
                model.ProjectContext.ProjectName = projectApiModel.project.projectName;
                model.ProjectContext.Description = projectApiModel.project.description;
                model.ProjectContext.Manager = projectApiModel.project.manager;
                model.ProjectContext.TimeStamp = formatDateToDateTime(long.Parse((string)projectApiModel.project.timeStamp));
            }
            Notify();
            return model;
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
                    project.ProjectId = projectApiModel.project.projectId;
                    project.ProjectName = projectApiModel.project.projectName;
                    project.Description = projectApiModel.project.description;
                    project.Manager = projectApiModel.project.manager;
                    project.TimeStamp = formatDateToDateTime(long.Parse((string)projectApiModel.project.timeStamp));
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
                        project.TimeStamp = formatDateToDateTime(long.Parse((string)o.timeStamp));
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
                        project.TimeStamp = formatDateToDateTime(long.Parse((string)o.timeStamp));
                        projectList.Add(project);
                    }
                }
            }
            return projectList;
        }

        public List<Project> getAllProjectList(int userId)
        {
            List<Project> projectList = new List<Project>();
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
                        project.TimeStamp = formatDateToDateTime(long.Parse((string)o.timeStamp));
                        projectList.Add(project);
                    }
                }
            }
            return projectList;
        }

        public ProjectApiModel updateProjectInfo(int userId, Project project)
        {
            ProjectApiModel model = new ProjectApiModel();
            var req = WebRequest.Create(Server.ApiUrl + "/projects/put/" + userId + "/" + project.ProjectId);
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
                model.State = (int)projectApiModel;
                model.State = 0;
                model.ProjectContext = getProjectInfo(userId, project.ProjectId);
            }
            Notify();
            return model;
        }

        public int deleteProject(int userId, int projectId)
        {
            int state;
            var req = WebRequest.Create(Server.ApiUrl + "/projects/delete/" + userId + "/" + projectId);
            req.Method = "GET";

            var resp = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(resp.GetResponseStream()))
            {
                var projectData = reader.ReadToEnd();
                dynamic projectApiModel = JsonConvert.DeserializeObject<dynamic>(projectData);
                state = (int)projectApiModel;
            }
            //Notify();
            return state;
        }

        private DateTime formatDateToDateTime(long date)
        {
            return new DateTime((new DateTime(1970, 1, 1, 0, 0, 0).Ticks) + date * 10000).ToLocalTime();
        }

        public void Notify()
        {
            if (projectDataChanged != null)
                projectDataChanged();
        }
    }
}
