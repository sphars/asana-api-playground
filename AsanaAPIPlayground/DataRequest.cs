using System;
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