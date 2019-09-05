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

    public class Team
    {
        public List<TeamData> Data { get; set; }
    }

    public class TeamData
    {
        public string Id { get; set; }
        public string Gid { get; set; }
        public string Name { get; set; }
        public string Resource_Type { get; set; }
    }

    public class Task
    {
        public TaskData Data { get; set; }
    }

    public class NewTask
    {
        [JsonProperty(PropertyName = "data")]
        public NewTaskData Data { get; set; }
    }

    public class Tasks
    {
        public List<TaskData> Data { get; set; }
    }

    public class TaskData
    {
        public string Id { get; set; }
        public string Gid { get; set; }
        public UserData Assignee { get; set; }
        public string Assignee_Status { get; set; }
        public bool Completed { get; set; }
        public DateTime? Completed_At { get; set; }
        public DateTime? Created_At { get; set; }
        public string[] Custom_Fields { get; set; }
        public DateTime? Due_At { get; set; }
        public DateTime? Due_On { get; set; }
        public List<UserData> Followers { get; set; }
        public bool Hearted { get; set; }
        public string[] Hearts { get; set; }
        public bool Liked { get; set; }
        public List<UserData> Likes { get; set; }
        public List<ProjectData> Memberships { get; set; }
        public DateTime? Modified_At { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int Num_Hearts { get; set; }
        public int Num_Likes { get; set; }
        public Task Parent { get; set; }
        public List<ProjectData> Projects { get; set; }
        public string Resource_Type { get; set; }
        public DateTime? Start_On { get; set; }
        public string[] Tags { get; set; }
        public string Resource_Subtype { get; set; }
        public Workspace Workspace { get; set; }
    }

    public class NewTaskData
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "due_on")]
        public DateTime? Due_On { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "projects")]
        public string Projects { get; set; }
        [JsonProperty(PropertyName = "assignee")]
        public string Assignee { get; set; }
    }

    public class Project
    {
        public ProjectData Data { get; set; }
    }

    public class ProjectData
    {
        public string Id { get; set; }
        public string Gid { get; set; }
        public bool Archived { get; set; }
        public string Color { get; set; }
        public DateTime? Created_At { get; set; }
        public Status Current_Status { get; set; }
        public string[] Custom_Fields { get; set; }
        public string[] Custom_Field_Settings { get; set; }
        public DateTime? Due_On { get; set; }
        public DateTime? Due_Date { get; set; }
        public List<UserData> Followers { get; set; }
        public bool Is_Template { get; set; }
        public string Layout { get; set; }
        public List<UserData> Members { get; set; }
        public DateTime? Modified_At { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public UserData Owner { get; set; }
        public bool Public { get; set; }
        public string Resource_Type { get; set; }
        public string Section_Migration_Status { get; set; }
        public DateTime? Start_On { get; set; }
        public TeamData Team { get; set; }
        public Workspace Workspace { get; set; }

    }

    public class Status
    {
        public string Id { get; set; }
        public string Gid { get; set; }
        public UserData Author { get; set; }
        public string Color { get; set; }
        public DateTime? Created_At { get; set; }
        public UserData Created_By { get; set; }
        public DateTime? Modified_At { get; set; }
        public string Resource_Type { get; set; }
        public string Text { get; set; }
    }
}
