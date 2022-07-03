using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lienophino.ApiModel;

public class ApiMealHistoryItem
{
    [BindRequired]
    public DateTime Date { get; set; }
    [BindRequired]
    public Guid MealId { get; set; }
}