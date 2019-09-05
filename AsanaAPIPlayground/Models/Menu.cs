using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsanaAPIPlayground
{
    public class MenuItem
    {
        public string Description { get; set; }
        public bool HasSubmenu { get; set; }
        public int? SubmenuID { get; set; }
        public Action Execute { get; set; }
    }

    public class Menu
    {
        public List<MenuItem> MenuItems;
        public int ID { get; set; }
        public string Title { get; set; }

        public Menu()
        {
            MenuItems = new List<MenuItem>();
        }

        public void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("---{0} Menu---", Title);
            foreach (MenuItem menuItem in MenuItems)
            {
                Console.WriteLine(" {0} - {1}", MenuItems.IndexOf(menuItem), menuItem.Description);
            }
        }

    }

    public class MenuCollection
    {
        public List<Menu> Menus { get; set; }

        public MenuCollection()
        {
            Menus = new List<Menu>();
        }

        public void ShowMenu(int id)
        {
            //get the menu we want to display and print it
            var currentMenu = Menus.Where(m => m.ID == id).Single();
            currentMenu.PrintMenu();

            //get user input
            Console.WriteLine();
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            //validate user input
            if (!int.TryParse(choice, out int choiceIndex) || currentMenu.MenuItems.Count <= choiceIndex || choiceIndex < 0)
            {
                Console.Clear();
                Console.WriteLine("Invalid selection.");
                ShowMenu(id);
            }
            else
            {
                //retrieve the selected choice
                var selectedMenuItem = currentMenu.MenuItems[choiceIndex];

                //check if there's a submenu
                if (selectedMenuItem.HasSubmenu)
                {
                    Console.Clear();
                    ShowMenu(selectedMenuItem.SubmenuID.Value);
                }
                else
                {
                    //execute the menu's action
                    selectedMenuItem.Execute();

                    //return to previous menu
                    ShowMenu(id); 
                }
            }
        }
    }

}