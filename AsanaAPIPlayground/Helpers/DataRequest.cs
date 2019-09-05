﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using RestSharp;
using RestSharp.Serialization;
using Newtonsoft.Json;

namespace AsanaAPIPlayground
{
    internal class AsanaApi
    {
        const string BaseUrl = "https://app.asana.com/api/1.0";

        private string _accessToken;

        readonly IRestClient _client;

        /// <summary>
        /// AsanaApi constructor
        /// </summary>
        public AsanaApi()
        {
            _client = new RestClient(BaseUrl).UseSerializer(() => new JsonNetSerializer());
            _accessToken = ConfigurationManager.AppSettings.Get("AsanaToken");
        }

        /// <summary>
        /// Generic Asana API request
        /// </summary>
        /// <typeparam name="T">The type of request</typeparam>
        /// <param name="request">The specific request</param>
        /// <returns>The serialized object from the request</returns>
        public T Execute<T>(RestRequest request) where T : new()
        {
            request.AddParameter("Authorization", string.Format("Bearer " + _accessToken), ParameterType.HttpHeader);
            var response = _client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more information";
                var asanaException = new ApplicationException(message, response.ErrorException);
                throw asanaException;
            }
            return response.Data;
        }

        /// <summary>
        /// Gets a single user
        /// </summary>
        /// <param name="userGid">The user's unique gid</param>
        /// <returns>A User object</returns>
        public User GetUser(string userGid)
        {
            var request = new RestRequest("/users/{userGid}", DataFormat.Json);
            request.AddUrlSegment("userGid", userGid);

            return Execute<User>(request);
        }

        /// <summary>
        /// Gets the teams that a user is a member of
        /// </summary>
        /// <param name="userGid">The user's unique gid</param>
        /// <param name="userOrg">The user's specified organization</param>
        /// <returns>A Team object</returns>
        public Team GetTeams(string userGid, string userOrg)
        {
            var request = new RestRequest("/users/{userGid}/teams", DataFormat.Json);
            request.AddUrlSegment("userGid", userGid);
            request.AddParameter("organization", userOrg);

            return Execute<Team>(request);
        }

        /// <summary>
        /// Gets a single project
        /// </summary>
        /// <param name="projectGid">The project's unique gid</param>
        /// <returns>A Project object</returns>
        public Project GetProject(string projectGid)
        {
            var request = new RestRequest("/projects/{projectGid}", DataFormat.Json);
            request.AddUrlSegment("projectGid", projectGid);

            return Execute<Project>(request);
        }

        /// <summary>
        /// Gets a list of task assigned to a project
        /// </summary>
        /// <param name="projectGid">The project's unique gid</param>
        /// <returns>A Tasks object</returns>
        public Tasks GetProjectTasks(string projectGid)
        {
            var request = new RestRequest("/projects/{projectGid}/tasks", DataFormat.Json);
            request.AddUrlSegment("projectGid", projectGid);

            return Execute<Tasks>(request);
        }

        /// <summary>
        /// Create a new task in a project
        /// </summary>
        /// <param name="taskData">The NewTask object</param>
        /// <param name="workspace">The user's workspace to add to</param>
        /// <returns>The created Task object</returns>
        public Task PostProjectTask(NewTask taskData, string workspace)
        {
            var request = new RestRequest("/tasks", Method.POST, DataFormat.Json);
            request.AddHeader("Content-Type", "application/json");

            //serialize the object
            request.AddJsonBody(taskData);
            request.AddQueryParameter("workspace", workspace);

            return Execute<Task>(request);
        }

    }

    public class AsanaRequest
    {
        private const string BaseUrl = "https://app.asana.com/api/1.0";
        private string AccessToken;

        public AsanaRequest()
        {
            AccessToken = ConfigurationManager.AppSettings.Get("AsanaToken");
        }

        public IRestClient Client { get; set; } = new RestClient(BaseUrl)
            .UseSerializer(() => new JsonNetSerializer());

        public IRestResponse<User> GetUser(string userId)
        {
            var request = new RestRequest("/users/{userId}/", DataFormat.Json);
            request.AddParameter("Authorization",
                string.Format("Bearer " + AccessToken), ParameterType.HttpHeader);
            request.AddUrlSegment("userId", userId);

            var response = Client.Get<User>(request);

            return response;
        }

        public IRestResponse<Team> GetTeams(string userGid, string userOrg)
        {
            var request = new RestRequest("/users/{userGid}/teams", DataFormat.Json);
            request.AddParameter("Authorization",
                string.Format("Bearer " + AccessToken), ParameterType.HttpHeader);
            request.AddUrlSegment("userGid", userGid);
            request.AddParameter("organization", userOrg);

            var response = Client.Get<Team>(request);

            return response;
        }

        public IRestResponse<Project> GetProject(string projectGid)
        {
            var request = new RestRequest("/projects/{projectGid}", DataFormat.Json);
            request.AddParameter("Authorization",
                string.Format("Bearer " + AccessToken), ParameterType.HttpHeader);
            request.AddUrlSegment("projectGid", projectGid);

            var response = Client.Get<Project>(request);

            return response;
        }

        public IRestResponse<Tasks> GetProjectTasks(string projectGid)
        {
            var request = new RestRequest("/projects/{projectGid}/tasks", DataFormat.Json);
            request.AddParameter("Authorization",
                string.Format("Bearer " + AccessToken), ParameterType.HttpHeader);
            request.AddUrlSegment("projectGid", projectGid);

            var response = Client.Get<Tasks>(request);

            return response;
        }

        public Task PostProjectTask(NewTask taskData, string workspace)
        {
            var request = new RestRequest("/tasks", Method.POST, DataFormat.Json);

            request.AddParameter("Authorization",
                string.Format("Bearer " + AccessToken), ParameterType.HttpHeader);

            request.AddHeader("Content-Type", "application/json");                       

            var content = JsonConvert.SerializeObject(taskData);
            request.AddParameter("application/json", content, ParameterType.RequestBody);
            request.AddQueryParameter("workspace", workspace);

            var response = Client.Execute<Task>(request);

            //request.AddObject(task);

            return response.Data;
        }
    }

    /// <summary>
    /// Custom serializer using NewtonSoft's JSON serializer
    /// </summary>
    public class JsonNetSerializer : IRestSerializer
    {
        public string Serialize(object obj) =>
        JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content);

        public string[] SupportedContentTypes { get; } =
        {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }

}