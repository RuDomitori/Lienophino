using System.ComponentModel.DataAnnotations;

namespace Lienophino.Core.Entities;

public class Ingredient
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public List<Meal2Ingredient> Meal2Ingredients { get; set; }
}