using System.Collections.Generic;
using RecipeManagement.Core;
using RecipeManagement.Core.Models;
using Xunit;

namespace RecipeManagement.Tests
{
    public class RecipeManagerTests
    {
        private static Recipe CreateRecipe(int id = 1)
        {
            Recipe recipe = new Recipe();
            recipe.Id = id;
            recipe.Name = "Recipe " + id;
            recipe.Ingredients = new List<string>();
            recipe.Ingredients.Add("Salt");
            recipe.Ingredients.Add("Pepper");
            recipe.Instructions = new List<string>();
            recipe.Instructions.Add("Mix");
            recipe.Instructions.Add("Serve");
            return recipe;
        }

        [Fact]
        public void DictionaryRejectsDuplicateIds()
        {
            RecipeManager manager = new RecipeManager();
            Assert.True(manager.AddRecipe(CreateRecipe(1)));
            Assert.False(manager.AddRecipe(CreateRecipe(1)));
            Assert.NotNull(manager.FindRecipe(1));
        }

        [Fact]
        public void RecipeLoaderReadsWrappedRecipesFile()
        {
            string path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, "{\"recipes\":[{\"id\":5,\"name\":\"Loaded\"}]}");

                List<Recipe> recipes = RecipeLoader.Load(path);

                Assert.Single(recipes);
                Assert.Equal(5, recipes[0].Id);
                Assert.Equal("Loaded", recipes[0].Name);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void ShoppingListCopiesIngredientsAndCanBeCleared()
        {
            RecipeManager manager = new RecipeManager();
            manager.AddRecipe(CreateRecipe());
            Assert.True(manager.AddRecipeToShoppingList(1));

            List<string> expected = new List<string>();
            expected.Add("Salt");
            expected.Add("Pepper");

            Assert.Equal(expected, manager.GetShoppingList());
            manager.ClearShoppingList();
            Assert.Empty(manager.GetShoppingList());
        }

        [Fact]
        public void CookingPlanRejectsDuplicateAndRestoresLastRemoved()
        {
            RecipeManager manager = new RecipeManager();
            manager.AddRecipe(CreateRecipe(1));
            manager.AddRecipe(CreateRecipe(2));

            Assert.True(manager.AddToCookingPlan(1));
            Assert.False(manager.AddToCookingPlan(1));
            Assert.True(manager.AddToCookingPlan(2));
            Assert.True(manager.RemoveFromCookingPlan(1));
            Assert.True(manager.RestoreLastRemovedRecipe());

            List<int> expected = new List<int>();
            expected.Add(2);
            expected.Add(1);

            Assert.Equal(expected, manager.GetCookingPlan());
        }

        [Fact]
        public void EmptyStackAndMissingRecipeAreSafe()
        {
            RecipeManager manager = new RecipeManager();
            Assert.False(manager.RestoreLastRemovedRecipe());
            Assert.False(manager.AddRecipeToShoppingList(99));
            Assert.Null(manager.FindRecipe(99));
        }

        [Fact]
        public void InstructionQueueProcessesInFifoOrderAndHandlesEmptyQueue()
        {
            RecipeManager manager = new RecipeManager();
            manager.AddRecipe(CreateRecipe());
            Assert.True(manager.StartCooking(1));
            Assert.Equal("Mix", manager.PeekNextInstruction());
            Assert.Equal("Mix", manager.CompleteNextInstruction());
            Assert.Equal("Serve", manager.CompleteNextInstruction());
            Assert.Null(manager.PeekNextInstruction());
            Assert.Null(manager.CompleteNextInstruction());
        }

        [Fact]
        public void MissingPlanAndCookingIdsReturnFalse()
        {
            RecipeManager manager = new RecipeManager();

            Assert.False(manager.AddToCookingPlan(99));
            Assert.False(manager.RemoveFromCookingPlan(99));
            Assert.False(manager.StartCooking(99));
        }

        [Fact]
        public void RestoringMultipleRemovedRecipesUsesLifoOrder()
        {
            RecipeManager manager = new RecipeManager();
            manager.AddRecipe(CreateRecipe(1));
            manager.AddRecipe(CreateRecipe(2));
            manager.AddRecipe(CreateRecipe(3));
            manager.AddToCookingPlan(1);
            manager.AddToCookingPlan(2);
            manager.AddToCookingPlan(3);

            manager.RemoveFromCookingPlan(1);
            manager.RemoveFromCookingPlan(2);

            Assert.True(manager.RestoreLastRemovedRecipe());
            Assert.True(manager.RestoreLastRemovedRecipe());

            List<int> expected = new List<int> { 3, 2, 1 };
            Assert.Equal(expected, manager.GetCookingPlan());
        }

        [Fact]
        public void RemovingRecipeAlsoRemovesItFromCookingPlan()
        {
            RecipeManager manager = new RecipeManager();
            manager.AddRecipe(CreateRecipe(1));
            manager.AddToCookingPlan(1);

            Assert.True(manager.RemoveRecipe(1));
            Assert.Empty(manager.GetCookingPlan());
            Assert.False(manager.StartCooking(1));
        }

        [Fact]
        public void RecipeMovesThroughCataloguePlanAndCookingWorkflow()
        {
            RecipeManager manager = new RecipeManager();
            manager.AddRecipe(CreateRecipe(7));
            Assert.True(manager.AddToCookingPlan(7));
            Assert.True(manager.StartCooking(7));
            Assert.Equal("Mix", manager.CompleteNextInstruction());
            Assert.True(manager.RemoveFromCookingPlan(7));
        }
    }
}
