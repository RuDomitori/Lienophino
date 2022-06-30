using Lienophino.ApiModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Lienophino.Tests;

public class FirstTests
{
    [Fact]
    public async Task CreateAndDeleteMeal()
    {
        // Arrange
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();
        
        // Act
        var mealCreatingResponse = await httpClient.PostAsJsonAsync("Meals", new 
        {
            Name = "Integration test meal",
            Description = "This meal is created for testing reason and must be not used in any other purposes"
        });
        var createdApiMeal = await mealCreatingResponse.Content.ReadFromJsonAsync<ApiMeal>();
        var mealsAfterCreate = await httpClient.GetFromJsonAsync<List<ApiMeal>>("Meals");
        var mealDeletingResponse = await httpClient.DeleteAsync($"Meals/{createdApiMeal.Id}");
        var mealsAfterDelete = await httpClient.GetFromJsonAsync<List<ApiMeal>>("Meals");

        // Assert
        Assert.NotNull(mealsAfterCreate);
        Assert.Contains(mealsAfterCreate, meal => meal.Id == createdApiMeal.Id);
        
        Assert.NotNull(mealsAfterDelete);
        Assert.DoesNotContain(mealsAfterDelete, meal => meal.Id == createdApiMeal.Id);
    }
}