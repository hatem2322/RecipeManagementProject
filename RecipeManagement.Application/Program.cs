using System;
using System.IO;
using System.Collections.Generic;
using RecipeManagement.Core;
using RecipeManagement.Core.Models;
using RecipeManagement.Application;

var manager = new RecipeManager();
var dataPath = Path.Combine(AppContext.BaseDirectory, "data", "recipes.json");

if (File.Exists(dataPath))
{
    foreach (var recipe in RecipeLoader.Load(dataPath))
    {
        manager.AddRecipe(recipe);
    }
}
else
{
    manager.AddRecipe(new Recipe
    {
        Id = 1,
        Name = "Simple Pasta",
        Ingredients = new List<string> { "200 g pasta", "1 tomato", "Salt" },
        Instructions = new List<string> { "Boil the pasta.", "Add the tomato.", "Serve." }
    });
}

var menu = new ConsoleMenu(manager);
menu.Show();
