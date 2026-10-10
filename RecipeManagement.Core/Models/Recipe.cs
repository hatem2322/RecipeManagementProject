namespace RecipeManagement.Core.Models;

public sealed class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
    public List<string> Instructions { get; set; } = new();
    public NutritionInfo Nutrition { get; set; } = new();
}
