using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsanaNet;
using AsanaNet.Objects;
using AsanaNet.Objects.New;

namespace AsanaAPIPlayground
{
    class Program
    {
        private const string _APITestProjectGid = "1122241621070635";
        private const string _APITestCentralTeamGid = "1140907983254353";
        private static MenuCollection menuCollection;
        public static Asana _asana;
        private static string _accessToken;
        private static AsanaUser self;


        static void Main(string[] args)
        {
            CreateMenus();

            Console.WriteLine("Welcome to the Asana API.");
            Console.WriteLine("-------------------------");

            _accessToken = ConfigurationManager.AppSettings.Get("AsanaToken");
            _asana = new Asana(_accessToken);

            GetSelf();

            //show the main menu
            menuCollection.ShowMenu(1);
        }

        static void CreateMenus()
        {
            menuCollection = new MenuCollection()
            {
                Menus =
                {
                    new Menu()
                    {
                        ID = 1,
                        Title = "Main",
                        MenuItems =
                        {
                            new MenuItem()
                            {
                                Description = "Users",
                                HasSubmenu = true,
                                SubmenuID = 2
                            },
                            new MenuItem()
                            {
                                Description = "Projects",
                                HasSubmenu = true,
                                SubmenuID = 3,
                            },
                            new MenuItem()
                            {
                                Description = "Exit",
                                HasSubmenu = false,
                                Execute = ExitApp
                            }
                        }
                    },
                    new Menu()
                    {
                        ID = 2,
                        Title = "Users",
                        MenuItems =
                        {
                            new MenuItem()
                            {
                                Description = "List my information",
                                HasSubmenu = false,
                                Execute = GetSelf
                            },
                            new MenuItem()
                            {
                                Description = "List my teams",
                                HasSubmenu = false,
                                Execute = GetTeams
                            },
                            new MenuItem()
                            {
                                Description = "Return to main menu",
                                HasSubmenu = true,
                                SubmenuID = 1
                            }
                        }
                    },
                    new Menu()
                    {
                        ID = 3,
                        Title = "Projects",
                        MenuItems =
                        {
                            new MenuItem()
                            {
                                Description = "Asana API Test Project",
                                HasSubmenu = true,
                                SubmenuID = 4
                            },
                            new MenuItem()
                            {
                                Description = "Add new project",
                                HasSubmenu = false,
                                Execute = PostProject
                            },
                            new MenuItem()
                            {
                                Description = "Return",
                                HasSubmenu = true,
                                SubmenuID = 2,
                                Execute = () => { }
                            }
                        }
                    },
                    new Menu()
                    {
                        ID = 4,
                        Title = "Asana API Test Project",
                        MenuItems =
                        {
                            new MenuItem()
                            {
                                Description = "Get Asana API Test Project Data",
                                HasSubmenu = false,
                                Execute = () =>
                                {
                                    GetProject(_APITestProjectGid);
                                }
                            },
                            new MenuItem()
                            {
                                Description = "List tasks",
                                HasSubmenu = false,
                                Execute = () =>
                                {
                                    GetProjectTasks(_APITestProjectGid);
                                }
                            },
                            new MenuItem()
                            {
                                Description = "Get task data",
                                HasSubmenu=false,
                                Execute = () =>
                                {
                                    GetTaskData("1132422945108960");
                                }
                            },
                            new MenuItem()
                            {
                                Description = "Add a task",
                                HasSubmenu = false,
                                Execute = PostTask
                            },
                            new MenuItem()
                            {
                                Description = "Batch add tasks",
                                HasSubmenu = false,
                                Execute = () =>
                                {
                                    BatchAddTasks(_APITestProjectGid);
                                }
                            },
                            new MenuItem()
                            {
                                Description = "Return",
                                HasSubmenu = true,
                                SubmenuID = 3
                            }
                        }
                    }
                }
            };
        }

        public static void ExitApp()
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }

        public static void GetSelf()
        {
            if (self == null)
            {
                //_asana = new Asana();
                self = _asana.GetUser("me");
            }
            else
            {
                PrintUser(self);
            }
        }

        public static void PrintUser(AsanaNet.Objects.AsanaUser user)
        {
            Console.WriteLine("Here's {0}'s information:", user.Name);
            Console.WriteLine("Name: {0}", self.Name);
            Console.WriteLine("Email: {0}", self.Email);
        }

        public static void GetUser(string userGid)
        {
            var user = _asana.GetUser(userGid);

            PrintUser(user);
        }

        public static void GetTeams()
        {
            //var teams = _asana.GetTeams(self.Data.Gid, self.Data.Workspaces[0].Gid);

            //Console.WriteLine("Here's {0}'s teams:", self.Data.Name);
            //foreach (TeamData team in teams.Data)
            //{
            //    Console.WriteLine("{0}", team.Name);
            //}
        }

        public static void GetAllUsers()
        {

        }

        public static void GetProject(string projectGid)
        {
            var project = _asana.GetProject(projectGid);

            Console.WriteLine("Here's {0}'s data:", project.Name);
            Console.WriteLine("Description: {0}", project.Notes);

            if (project.CurrentStatus != null)
            {
                Console.Write("Current Status: ");
                switch (project.CurrentStatus.Color)
                {
                    case "green":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "yellow":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case "red":
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    default:
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine(project.CurrentStatus.Text);
                Console.ResetColor();
            }

            Console.WriteLine("Project members:");
            foreach (AsanaUser user in project.Members.OrderBy(o => o.Name).ToList())
            {
                Console.WriteLine("  {0}", user.Name);
            }

        }

        public static void GetProjectTasks(string projectGid)
        {
            //var tasks = _asana.GetProjectTasks(projectGid);
            var tasks = _asana.GetTasks(projectGid, compact: false);

            Console.WriteLine("Task list:");
            foreach (AsanaNet.Objects.AsanaTask task in tasks)
            {
                Console.WriteLine(" {0}", task.Name);
                Console.WriteLine("    {0}", task.Notes);
                if (task.Assignee != null)
                    Console.WriteLine("    Assigned to: {0}", task.Assignee.Name);
                if (task.DueOn != null)
                    Console.WriteLine("    Due: {0}", DateTime.Parse(task.DueOn.ToString()).ToString("MMMM dd, yyyy"));
                Console.WriteLine("    Completed: {0}", task.Completed);
            }
        }

        public static void GetTaskData(string taskGid)
        {
            AsanaTask task = _asana.GetTaskData(taskGid);

            Console.WriteLine(task.Name);
        }

        public static void PostTask()
        {
            var createdTask = _asana.PostProjectTask(CreateNewTask(), _APITestProjectGid);

            Console.WriteLine(createdTask.Name);
        }

        public static void PostProject()
        {
            var createdProject = _asana.PostProject(CreateNewProject(), _APITestCentralTeamGid);

            Console.WriteLine(createdProject.Name);
        }

        public static NewAsanaTask CreateNewTask()
        {
            var newTask = new NewAsanaTask
            {
                Followers = new List<string>()
            };

            Console.Write("Enter task name: ");
            newTask.Name = Console.ReadLine();

            Console.Write("Enter due date (leave blank for none): ");
            string dateInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(dateInput))
            {
                DateTime dueDate;
                while (!DateTime.TryParse(dateInput, out dueDate))
                {
                    Console.Write("Invalid date. Try again: ");
                }
                newTask.DueOn = dueDate;
            }

            Console.Write("Enter task notes: ");
            newTask.Notes = Console.ReadLine();

            Console.Write("Enter assignee gid (leave blank for none, 'me' for self): ");
            string assignee = Console.ReadLine();
            if (!string.IsNullOrEmpty(assignee))
            {
                newTask.Assignee = assignee;
            }

            while (true)
            {
                Console.Write("Add follower email (blank to stop): ");
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    newTask.Followers.Add(input);
                    continue;
                }
                else
                {
                    break;
                }
            }

            return newTask;
        }

        public static NewAsanaProject CreateNewProject()
        {
            var newProject = new NewAsanaProject();

            Console.Write("Enter task name: ");
            newProject.Name = Console.ReadLine();


            Console.Write("Enter task notes: ");
            newProject.Notes = Console.ReadLine();


            Console.Write("Enter a color: ");
            newProject.Color = Console.ReadLine();

            return newProject;
        }


        public static void BatchAddTasks(string projectGid)
        {
            //var batchData = new Batch
            //{
            //    data = new BatchData
            //    {
            //        actions = new List<AsanaActionTask>()
            //    }
            //};

            //Console.WriteLine("Create a new task");
            //Console.WriteLine("Enter 'n' to exit");

            //while (true)
            //{
            //    Console.WriteLine("----");
            //    var newTask = CreateNewTask(projectGid);

            //    batchData.data.actions.Add(new AsanaActionTask
            //    {
            //        relative_path = "/tasks",
            //        method = "post",
            //        data = newTask.data
            //    });

            //    Console.Write("Another? y/n: ");
            //    var input = Console.ReadLine();
            //    if (input.ToLower() == "y")
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            //var batchResponse = _asana.PostProjectTasksBatch(batchData);

            //Console.WriteLine();

        }
    }
}
