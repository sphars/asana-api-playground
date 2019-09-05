using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsanaAPIPlayground
{
    class Program
    {
        private static MenuCollection menuCollection;
        private static AsanaApi _asanaApi;
        private static User self;

        static void Main(string[] args)
        {
            CreateMenus();

            Console.WriteLine("Welcome to the Asana API.");
            Console.WriteLine("-------------------------");

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
                _asanaApi = new AsanaApi();
                self = _asanaApi.GetUser("me");
            }
            else
            {
                PrintUser(self);
            }
        }

        public static void PrintUser(User user)
        {
            Console.WriteLine("Here's {0}'s information:", user.Data.Name);
            Console.WriteLine("Name: {0}", self.Data.Name);
            Console.WriteLine("Email: {0}", self.Data.Email);
        }

        public static void GetUser(string userGid)
        {
            _asanaApi = new AsanaApi();
            var user = _asanaApi.GetUser(userGid);

            PrintUser(user);
        }

        public static void GetTeams()
        {
            _asanaApi = new AsanaApi();
            var teams = _asanaApi.GetTeams(self.Data.Gid, self.Data.Workspaces[0].Gid);

            Console.WriteLine("Here's {0}'s teams:", self.Data.Name);
            foreach (TeamData team in teams.Data)
            {
                Console.WriteLine("{0}", team.Name);
            }
        }

        public static void GetAllUsers()
        {

        }

        public static void GetProject(string projectGid)
        {
            _asanaApi = new AsanaApi();
            var project = _asanaApi.GetProject(projectGid);

            Console.WriteLine("Here's {0}'s data:", project.Data.Name);
            Console.Write("Current Status: ");
            switch (project.Data.Current_Status.Color)
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
            Console.WriteLine(project.Data.Current_Status.Text);
            Console.ResetColor();
        }

        public static void GetProjectTasks(string projectGid)
        {
            _asanaApi = new AsanaApi();
            var tasks = _asanaApi.GetProjectTasks(projectGid);

            Console.WriteLine("Task list:");
            foreach (TaskData task in tasks.Data)
            {
                Console.WriteLine(" {0}", task.Name);
            }
        }

        public static void PostTask()
        {
            NewTask newTask = new NewTask();

            Console.Write("Enter project GID: ");
            newTask.Data.Projects = Console.ReadLine();

            Console.Write("Enter task name: ");
            newTask.Data.Name = Console.ReadLine();

            Console.Write("Enter due date (leave blank for none) yyyy-mm-dd: ");
            newTask.Data.Due_On = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter task notes: ");
            newTask.Data.Notes = Console.ReadLine();

            //Console.Write("Enter assignee (leave blank for none): ");
            //string assignee = Console.ReadLine();

            _asanaApi = new AsanaApi();
            var createdTask = _asanaApi.PostProjectTask(newTask, self.Data.Workspaces[0].Gid);

        }
    }
}
