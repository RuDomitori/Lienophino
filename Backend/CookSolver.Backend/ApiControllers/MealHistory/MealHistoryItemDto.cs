using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CookSolver.ApiControllers.MealHistory;

public class MealHistoryItemDto
{
    [BindRequired]
    public DateTime Date { get; set; }
    [BindRequired]
    public Guid MealId { get; set; }
}