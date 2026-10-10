# Recipe Management

A runnable C# console solution demonstrating five fundamental collection types:
`Dictionary<int, Recipe>`, `List<string>`, `LinkedList<int>`, `Stack<int>`, and `Queue<string>`.

## Projects

- `RecipeManagement.Core` — recipe models, JSON loading, the public interface, and manager implementation.
- `RecipeManagement.Application` — interactive console application.
- `RecipeManagement.Tests` — xUnit tests covering the catalogue, shopping list, cooking plan, removal history, instruction queue, and integration workflows.
- `data/recipes.json` — sample recipe data stored at the repository root.

## Build and run

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application
```

The application copies the root `data/recipes.json` file to its output directory during the build.

## AI acknowledgement

AI assistance was used during development for project scaffolding, code review, test planning, and documentation suggestions. The student remains responsible for reviewing, understanding, testing, and explaining the submitted code.
