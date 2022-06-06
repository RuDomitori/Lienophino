using CookSolver.ApiModel;

namespace CookSolver.ApiControllers.MealHistory;

public class MealHistoryChangesDto
{
    public List<ApiMealHistoryItem> ToAdd { get; set; } = new();
    public List<ApiMealHistoryItem> ToDelete { get; set; } = new();
}