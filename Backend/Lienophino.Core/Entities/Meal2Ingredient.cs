namespace Lienophino.Core.Entities;

public class Meal2Ingredient
{
    public Guid MealId { get; set; }
    public Guid IngredientId { get; set; }

    public Meal Meal { get; set; }
    public Ingredient Ingredient { get; set; }
}