using System;
using System.Collections.Generic;
using RecipeManagement.Core;
using RecipeManagement.Core.Models;

namespace RecipeManagement.Application
{
    public class ConsoleMenu
    {
        private RecipeManager manager;

        public ConsoleMenu(RecipeManager manager)
        {
            this.manager = manager;
        }

        public void Show()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("--- Recipe Management Menu ---");
                Console.WriteLine("1. List all recipes");
                Console.WriteLine("2. Find recipe by id");
                Console.WriteLine("3. Add recipe to shopping list");
                Console.WriteLine("4. View shopping list");
                Console.WriteLine("5. Clear shopping list");
                Console.WriteLine("6. Add recipe to cooking plan");
                Console.WriteLine("7. View cooking plan");
                Console.WriteLine("8. Remove recipe from cooking plan");
                Console.WriteLine("9. Restore last removed recipe");
                Console.WriteLine("10. Start cooking");
                Console.WriteLine("11. Show next instruction");
                Console.WriteLine("12. Complete next instruction");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    running = false;
                }
                else if (choice == "1")
                {
                    ListRecipes();
                }
                else if (choice == "2")
                {
                    FindRecipe();
                }
                else if (choice == "3")
                {
                    AddToShoppingList();
                }
                else if (choice == "4")
                {
                    ViewShoppingList();
                }
                else if (choice == "5")
                {
                    manager.ClearShoppingList();
                    Console.WriteLine("Shopping list cleared.");
                }
                else if (choice == "6")
                {
                    AddToCookingPlan();
                }
                else if (choice == "7")
                {
                    ViewCookingPlan();
                }
                else if (choice == "8")
                {
                    RemoveFromCookingPlan();
                }
                else if (choice == "9")
                {
                    bool ok = manager.RestoreLastRemovedRecipe();
                    if (ok == true)
                        Console.WriteLine("Restored.");
                    else
                        Console.WriteLine("Nothing to restore.");
                }
                else if (choice == "10")
                {
                    StartCooking();
                }
                else if (choice == "11")
                {
                    string next = manager.PeekNextInstruction();
                    if (next == null)
                        Console.WriteLine("No instruction.");
                    else
                        Console.WriteLine("Next: " + next);
                }
                else if (choice == "12")
                {
                    string done = manager.CompleteNextInstruction();
                    if (done == null)
                        Console.WriteLine("No instruction.");
                    else
                        Console.WriteLine("Done: " + done);
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }
            }
        }

        private void ListRecipes()
        {
            List<Recipe> recipes = (List<Recipe>)manager.GetRecipes();

            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes.");
                return;
            }

            foreach (Recipe r in recipes)
            {
                Console.WriteLine(r.Id + ": " + r.Name);
            }
        }

        private void FindRecipe()
        {
            Console.Write("Enter id: ");
            int id = int.Parse(Console.ReadLine());

            Recipe r = manager.FindRecipe(id);

            if (r == null)
            {
                Console.WriteLine("Not found.");
            }
            else
            {
                Console.WriteLine("Name: " + r.Name);

                Console.Write("Ingredients: ");
                foreach (string item in r.Ingredients)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                Console.Write("Instructions: ");
                foreach (string item in r.Instructions)
                {
                    Console.Write(item + " | ");
                }
                Console.WriteLine();
            }
        }

        private void AddToShoppingList()
        {
            Console.Write("Enter recipe id: ");
            int id = int.Parse(Console.ReadLine());

            bool ok = manager.AddRecipeToShoppingList(id);

            if (ok == true)
                Console.WriteLine("Added.");
            else
                Console.WriteLine("Recipe not found.");
        }

        private void ViewShoppingList()
        {
            List<string> list = (List<string>)manager.GetShoppingList();

            if (list.Count == 0)
            {
                Console.WriteLine("Shopping list is empty.");
                return;
            }

            foreach (string item in list)
            {
                Console.WriteLine("- " + item);
            }
        }

        private void AddToCookingPlan()
        {
            Console.Write("Enter recipe id: ");
            int id = int.Parse(Console.ReadLine());

            bool ok = manager.AddToCookingPlan(id);

            if (ok == true)
                Console.WriteLine("Added to plan.");
            else
                Console.WriteLine("Cannot add.");
        }

        private void ViewCookingPlan()
        {
            List<int> plan = (List<int>)manager.GetCookingPlan();

            if (plan.Count == 0)
            {
                Console.WriteLine("Cooking plan is empty.");
                return;
            }

            Console.Write("Plan: ");
            foreach (int id in plan)
            {
                Console.Write(id + " ");
            }
            Console.WriteLine();
        }

        private void RemoveFromCookingPlan()
        {
            Console.Write("Enter recipe id: ");
            int id = int.Parse(Console.ReadLine());

            bool ok = manager.RemoveFromCookingPlan(id);

            if (ok == true)
                Console.WriteLine("Removed.");
            else
                Console.WriteLine("Not in plan.");
        }

        private void StartCooking()
        {
            Console.Write("Enter recipe id: ");
            int id = int.Parse(Console.ReadLine());

            bool ok = manager.StartCooking(id);

            if (ok == true)
                Console.WriteLine("Cooking started.");
            else
                Console.WriteLine("Recipe not found.");
        }
    }
}