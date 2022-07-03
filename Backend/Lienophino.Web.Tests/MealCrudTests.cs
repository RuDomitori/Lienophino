using Lienophino.ApiModel;
using Lienophino.Web.Tests.Ordering;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Lienophino.Web.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class IntegrationTestsFixture : IAsyncLifetime
{
    internal WebApplicationFactory<Program> WebAppFactory;
    internal Guid? CreatedMealId;
    
    public async Task InitializeAsync()
    {
        WebAppFactory = new WebApplicationFactory<Program>();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await WebAppFactory.DisposeAsync();
    }
}

// Set the orderer
[TestCaseOrderer("Lienophino.Web.Tests.Ordering.DependenceOrderer", "Lienophino.Web.Tests")]
// Need to turn off test parallelization so we can validate the run order
[CollectionDefinition(nameof(MealCrudTests), DisableParallelization = true)]
public sealed class MealCrudTests: IClassFixture<IntegrationTestsFixture>
{
    private readonly IntegrationTestsFixture _fixture;
    
    public MealCrudTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task CreateMeal()
    {
        // Arrange
        var webAppFactory = _fixture.WebAppFactory;
        var httpClient = webAppFactory.CreateDefaultClient();
        
        // Act
        var mealCreatingResponse = await httpClient.PostAsJsonAsync("Meals", new 
        {
            Name = "Integration test meal",
            Description = "This meal is created for testing reason and must be not used in any other purposes"
        });
        var createdApiMeal = await mealCreatingResponse.Content.ReadFromJsonAsync<ApiMeal>();
        
        // Assert
        Assert.NotNull(createdApiMeal);
        
        _fixture.CreatedMealId = createdApiMeal.Id;
        
        // Act
        var mealsAfterCreate = await httpClient.GetFromJsonAsync<List<ApiMeal>>("Meals");
        
        // Assert
        Assert.NotNull(mealsAfterCreate);
        Assert.Contains(mealsAfterCreate, meal => meal.Id == createdApiMeal.Id);
    }
    
    [Fact]
    [DependenceOn(nameof(CreateMeal))]
    public async Task ChangeMeal()
    {
        // Precondition
        Assert.NotNull(_fixture.CreatedMealId);
        
        // Arrange
        var webAppFactory = _fixture.WebAppFactory;
        var httpClient = webAppFactory.CreateDefaultClient();
        var createdMealId = _fixture.CreatedMealId.Value;
        
        // Act
        var meals = await httpClient.GetFromJsonAsync<List<ApiMeal.WithNavProps>>("Meals?IncludeMealTags=true&IncludeIngredients=true");
        var meal = meals!.FirstOrDefault(x => x.Id == createdMealId)!;
        
        var newMealName = meal.Name + " With changed name.";
        var newMealDescription = meal.Description + " With changed description";

        var updateResponse = await httpClient.PutAsJsonAsync($"Meals/{createdMealId}", new
        {
            Name = newMealName,
            Description = newMealDescription,
            MealTagIds = meal.MealTags.Select(x => x.Id),
            IngredientIds = meal.Ingredients.Select(x => x.Id)
        });

        var updatedMeal = await updateResponse.Content.ReadFromJsonAsync<ApiMeal.WithNavProps>();

        // Assert
        Assert.NotNull(updatedMeal);
        Assert.Equal(newMealName, updatedMeal.Name);
        Assert.Equal(newMealDescription, updatedMeal.Description);
        
        // Act
        meals = await httpClient.GetFromJsonAsync<List<ApiMeal.WithNavProps>>("Meals?IncludeMealTags=true&IncludeIngredients=true");
        updatedMeal = meals!.FirstOrDefault(x => x.Id == createdMealId)!;
        
        // Assert
        Assert.NotNull(meal);
        Assert.Equal(newMealName, updatedMeal.Name);
        Assert.Equal(newMealDescription, updatedMeal.Description);
    }

    [Fact]
    [DependenceOn(nameof(ChangeMeal))]
    public async Task DeleteMeal()
    {
        // Precondition
        Assert.NotNull(_fixture.CreatedMealId);
        
        // Arrange
        var webAppFactory = _fixture.WebAppFactory;
        var httpClient = webAppFactory.CreateDefaultClient();
        var createdMealId = _fixture.CreatedMealId.Value;
        
        // Act
        var mealDeletingResponse = await httpClient.DeleteAsync($"Meals/{createdMealId}");
        var deletedApiMeal = await mealDeletingResponse.Content.ReadFromJsonAsync<ApiMeal>();
        
        // Assert
        Assert.NotNull(deletedApiMeal);
        Assert.Equal(createdMealId, deletedApiMeal.Id);
        
        // Act
        var mealsAfterDelete = await httpClient.GetFromJsonAsync<List<ApiMeal>>("Meals");
        
        // Assert
        Assert.NotNull(mealsAfterDelete);
        Assert.DoesNotContain(mealsAfterDelete, meal => meal.Id == createdMealId);
    }
}