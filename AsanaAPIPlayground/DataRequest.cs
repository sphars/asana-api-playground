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