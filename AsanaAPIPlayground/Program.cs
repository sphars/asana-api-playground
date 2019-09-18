using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsanaNet;
using AsanaNet.Models;

namespace AsanaAPIPlayground
{
    class Program
    {
        private static MenuCollection menuCollection;
        public static Asana _asana;
        private static string _accessToken;
        private static User self;

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
                                    GetProject("1122241621070635");
                                }
                            },
                            new MenuItem()
                            {
                                Description = "List tasks",
                                HasSubmenu = false,
                                Execute = () =>
                                {
                                    GetProjectTasks("1122241621070635");
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

        public static void PrintUser(User user)
        {
            Console.WriteLine("Here's {0}'s information:", user.data.name);
            Console.WriteLine("Name: {0}", self.data.name);
            Console.WriteLine("Email: {0}", self.data.email);
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

            Console.WriteLine("Here's {0}'s data:", project.data.name);
            Console.WriteLine("Description: {0}", project.data.notes);

            if (project.data.current_status != null)
            {
                Console.Write("Current Status: ");
                switch (project.data.current_status.color)
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
                Console.WriteLine(project.data.current_status.text);
                Console.ResetColor();
            }

            Console.WriteLine("Project members:");
            foreach(UserData user in project.data.members.OrderBy(o => o.name).ToList())
            {
                Console.WriteLine("  {0}", user.name);
            }
            
        }

        public static void GetProjectTasks(string projectGid)
        {
            //var tasks = _asana.GetProjectTasks(projectGid);
            var tasks = _asana.GetTasks(projectGid, compact:false);

            Console.WriteLine("Task list:");
            foreach (AsanaNet.Models.TaskData task in tasks.data)
            {
                Console.WriteLine(" {0}", task.name);
                Console.WriteLine("    {0}", task.notes);
                if (task.assignee != null)
                    Console.WriteLine("    Assigned to: {0}", task.assignee.name);
                if (task.due_on != null)
                    Console.WriteLine("    Due: {0}", DateTime.Parse(task.due_on.ToString()).ToString("MMMM dd, yyyy"));
                Console.WriteLine("    Completed: {0}", task.completed);
            }
        }

        public static void GetTaskData(string taskGid)
        {
            Task2 taskData = _asana.GetTaskData(taskGid);

            Console.WriteLine(taskData.data.name);
        }

        public static void PostTask()
        {
            //NewTask newTask = new NewTask();
            //newTask.Data = new NewTaskData();

            //Console.Write("Enter project GID: ");
            //newTask.Data.Projects = Console.ReadLine();

            //Console.Write("Enter task name: ");
            //newTask.Data.Name = Console.ReadLine();

            //Console.Write("Enter due date (leave blank for none): ");
            //string dateInput = Console.ReadLine();

            //if (!string.IsNullOrEmpty(dateInput))
            //{
            //    DateTime dueDate;
            //    while (!DateTime.TryParse(dateInput, out dueDate))
            //    {
            //        Console.Write("Invalid date. Try again: ");
            //    }
            //    newTask.Data.Due_On = dueDate;
            //}

            //Console.Write("Enter task notes: ");
            //newTask.Data.Notes = Console.ReadLine();

            ////Console.Write("Enter assignee (leave blank for none): ");
            ////string assignee = Console.ReadLine();

            //_asana = new AsanaApi();
            //var createdTask = _asana.PostProjectTask(newTask, self.Data.Workspaces[0].Gid);

        }
    }
}
