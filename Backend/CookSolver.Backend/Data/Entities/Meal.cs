using System.ComponentModel.DataAnnotations;

namespace CookSolver.Data.Entities;

public class Meal
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<MealHistoryItem> MealHistoryItems { get; set; }
}