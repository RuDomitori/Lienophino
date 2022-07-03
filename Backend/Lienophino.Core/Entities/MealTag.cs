using System.ComponentModel.DataAnnotations;

namespace Lienophino.Core.Entities;

public class MealTag
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    
    public List<Meal2MealTag> Meal2MealTags { get; set; }
}