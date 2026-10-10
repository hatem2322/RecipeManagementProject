using System;
using System.Collections.Generic;
using RecipeManagement.Core.Models;

namespace RecipeManagement.Core
{
    public sealed class RecipeManager : IRecipeManager
    {
        private readonly Dictionary<int, Recipe> recipes = new Dictionary<int, Recipe>();
        private readonly List<string> shoppingList = new List<string>();
        private readonly LinkedList<int> cookingPlan = new LinkedList<int>();
        private readonly Stack<int> recentlyRemoved = new Stack<int>();
        private readonly Queue<string> instructionQueue = new Queue<string>();
        private int jsoncount;

        public bool AddRecipe(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            if (recipes.ContainsKey(recipe.Id))
                return false;

            recipes.Add(recipe.Id, recipe);
            jsoncount = recipes.Count;
            return true;
        }

        public Recipe? FindRecipe(int recipeId)
        {
            if (recipes.ContainsKey(recipeId))
                return recipes[recipeId];

            return null;
        }

        public bool RemoveRecipe(int recipeId)
        {
            if (!recipes.ContainsKey(recipeId))
                return false;

            recipes.Remove(recipeId);
            cookingPlan.Remove(recipeId);
            jsoncount = recipes.Count;
            return true;
        }

        public IReadOnlyList<Recipe> GetRecipes()
        {
            List<Recipe> result = new List<Recipe>(jsoncount);
            result.AddRange(recipes.Values);
            return result;
        }

        public bool AddRecipeToShoppingList(int recipeId)
        {
            Recipe? recipe = FindRecipe(recipeId);

            if (recipe == null)
                return false;

            foreach (string ingredient in recipe.Ingredients)
            {
                shoppingList.Add(ingredient);
            }

            return true;
        }

        public IReadOnlyList<string> GetShoppingList()
        {
            return new List<string>(shoppingList);
        }

        public void ClearShoppingList()
        {
            shoppingList.Clear();
        }

        public bool AddToCookingPlan(int recipeId)
        {
            if (FindRecipe(recipeId) == null)
                return false;

            if (cookingPlan.Contains(recipeId))
                return false;

            cookingPlan.AddLast(recipeId);
            return true;
        }

        public bool RemoveFromCookingPlan(int recipeId)
        {
            LinkedListNode<int>? node = cookingPlan.Find(recipeId);

            if (node == null)
                return false;

            cookingPlan.Remove(node);
            recentlyRemoved.Push(recipeId);
            return true;
        }

        public IReadOnlyList<int> GetCookingPlan()
        {
            return new List<int>(cookingPlan);
        }

        public bool RestoreLastRemovedRecipe()
        {
            if (recentlyRemoved.Count == 0)
                return false;

            int recipeId = recentlyRemoved.Peek();

            if (cookingPlan.Contains(recipeId))
                return false;

            recentlyRemoved.Pop();
            cookingPlan.AddLast(recipeId);
            return true;
        }

        public bool StartCooking(int recipeId)
        {
            Recipe? recipe = FindRecipe(recipeId);

            if (recipe == null)
                return false;

            instructionQueue.Clear();

            foreach (string instruction in recipe.Instructions)
            {
                instructionQueue.Enqueue(instruction);
            }

            return true;
        }

        public string? PeekNextInstruction()
        {
            if (instructionQueue.Count == 0)
                return null;

            return instructionQueue.Peek();
        }

        public string? CompleteNextInstruction()
        {
            if (instructionQueue.Count == 0)
                return null;

            return instructionQueue.Dequeue();
        }
    }
}
