using RecipeManagement.Core.Models;

namespace RecipeManagement.Core;

public interface IRecipeManager
{
    bool AddRecipe(Recipe recipe);
    Recipe? FindRecipe(int recipeId);
    bool RemoveRecipe(int recipeId);
    IReadOnlyList<Recipe> GetRecipes();

    bool AddRecipeToShoppingList(int recipeId);
    IReadOnlyList<string> GetShoppingList();
    void ClearShoppingList();

    bool AddToCookingPlan(int recipeId);
    bool RemoveFromCookingPlan(int recipeId);
    IReadOnlyList<int> GetCookingPlan();
    bool RestoreLastRemovedRecipe();

    bool StartCooking(int recipeId);
    string? PeekNextInstruction();
    string? CompleteNextInstruction();
}
