namespace Lienophino.Data.Entities;

public class Meal2MealTag
{
    public Guid MealId { get; set; }
    public Guid MealTagId { get; set; }
    
    public Meal Meal { get; set; }
    public MealTag MealTag { get; set; }
}