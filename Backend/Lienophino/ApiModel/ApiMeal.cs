namespace Lienophino.ApiModel;

public class ApiMeal
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public class WithNavProps : ApiMeal
    {
        public List<ApiMealTag> MealTags { get; set; }
        public List<ApiIngredient> Ingredients { get; set; }
    }
}