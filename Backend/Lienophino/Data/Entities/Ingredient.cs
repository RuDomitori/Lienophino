using System.ComponentModel.DataAnnotations;

namespace Lienophino.Data.Entities;

public class Ingredient
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public List<Meal2Ingredient> Meal2Ingredients { get; set; }
}