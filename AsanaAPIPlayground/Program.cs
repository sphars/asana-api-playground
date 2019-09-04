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
        private static bool runApp = true;
        private static AsanaRequest asanaRequest;

        static void Main(string[] args)
        {
            CreateMenus();

            Console.WriteLine("Welcome to the Asana API.");
            Console.WriteLine("------------------------------");

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
                                Description = "Show your user info",
                                HasSubmenu = false,
                                Execute = GetSelf
                            },
                            new MenuItem()
                            {
                                Description = "List projects",
                                HasSubmenu = true,
                                SubmenuID = 2,
                                Execute = () =>
                                {

                                }
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
                        Title = "Projects",
                        MenuItems =
                        {
                            new MenuItem()
                            {
                                Description = "Project 1",
                                HasSubmenu = false,
                                Execute = () =>
                                {
                                    Console.WriteLine("I'm in project 1 action");
                                }
                            },
                            new MenuItem()
                            {
                                Description = "Project 2",
                                HasSubmenu = true,
                                SubmenuID = 3,
                                Execute = () => { }
                            },
                            new MenuItem()
                            {
                                Description = "Return to main menu",
                                HasSubmenu = true,
                                SubmenuID = 1,
                                Execute = () => { }
                            }
                        }
                    },
                    new Menu()
                    {
                        ID = 3,
                        Title = "Project 2",
                        MenuItems =
                        {
                            new MenuItem()
                            {
                                Description = "Display info",
                                HasSubmenu = false,
                                Execute = () =>
                                {
                                    Console.WriteLine("Project 2 information");
                                }
                            },
                            new MenuItem()
                            {
                                Description = "Return",
                                HasSubmenu = true,
                                SubmenuID = 2,
                                Execute = () => { }
                            }
                        }
                    }
                }
            };
        }

        public static void ExitApp()
        {
            Console.WriteLine("Exiting...");
            runApp = false;
            Environment.Exit(0);
        }

        public static void GetSelf()
        {

            asanaRequest = new AsanaRequest();
            var self = asanaRequest.GetUser("me").Data;

            Console.WriteLine("Here's you:");
            Console.WriteLine("Name: {0}", self.Data.Name);
            Console.WriteLine("Email: {0}", self.Data.Email);
        }
    }
}
