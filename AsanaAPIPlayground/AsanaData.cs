using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization;
using Newtonsoft.Json;

namespace AsanaAPIPlayground
{

    public class User
    {
        public UserData Data { get; set; }
    }

    public class UserData
    {
        public string Id { get; set; }
        public string Gid { get; set; }
        public string Resource_Type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Photo Photo { get; set; }
        public List<Workspace> Workspaces { get; set; }
    }

    public class Photo
    {
        public string image_21x21 { get; set; }
        public string image_27x27 { get; set; }
        public string image_36x36 { get; set; }
        public string image_60x60 { get; set; }
        public string image_128x128 { get; set; }
    }

    public class Workspace
    {
        public string Id { get; set; }
        public string Gid { get; set; }
        public string Name { get; set; }
        public string Resource_type { get; set; }
    }
}
