namespace CookSolver.Data.Entities;

public class MealHistoryItem
{
    public Guid MealId { get; set; }
    public DateOnly Date { get; set; }
    
    public Meal Meal { get; set; }
}