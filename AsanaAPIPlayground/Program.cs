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
        private static AsanaRequest asanaRequest;
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
                asanaRequest = new AsanaRequest();
                self = asanaRequest.GetUser("me").Data;
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
            asanaRequest = new AsanaRequest();
            var user = asanaRequest.GetUser(userGid).Data;

            PrintUser(user);
        }

        public static void GetTeams()
        {
            asanaRequest = new AsanaRequest();
            var teams = asanaRequest.GetTeams(self.Data.Gid, self.Data.Workspaces[0].Gid).Data;

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
            asanaRequest = new AsanaRequest();
            var project = asanaRequest.GetProject(projectGid).Data;

            Console.WriteLine("Here's {0}'s data:", project.Data.Name);
            Console.Write("Current Status: ");
            Console.ForegroundColor = project.Data.Current_Status.Color == "green" ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(project.Data.Current_Status.Text);
            Console.ResetColor();
        }

        public static void GetProjectTasks(string projectGid)
        {
            asanaRequest = new AsanaRequest();
            var tasks = asanaRequest.GetProjectTasks(projectGid).Data;

            Console.WriteLine("Task list:");
            foreach (TaskData task in tasks.Data)
            {
                Console.WriteLine(" {0}", task.Name);
            }
        }

        public static void PostTask()
        {
            Console.Write("Enter project GID: ");
            string projectGid = Console.ReadLine();
            Console.Write("Enter task name: ");
            string taskName = Console.ReadLine();
            Console.Write("Enter due date (leave blank for none) yyyy-mm-dd: ");
            string dueDate = Console.ReadLine();
            Console.Write("Enter task notes: ");
            string notes = Console.ReadLine();
            //Console.Write("Enter assignee (leave blank for none): ");
            //string assignee = Console.ReadLine();

            NewTask newTask = new NewTask
            {
                Data = new NewTaskData
                {
                    Name = taskName,
                    Due_On = DateTime.Parse(dueDate),
                    Notes = notes,
                    Projects = projectGid
                }                
            };

            var createdTask = asanaRequest.PostProjectTask(newTask, self.Data.Workspaces[0].Gid);
        }
    }
}
