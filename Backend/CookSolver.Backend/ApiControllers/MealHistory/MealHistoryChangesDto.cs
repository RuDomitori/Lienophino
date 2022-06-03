namespace CookSolver.ApiControllers.MealHistory;

public class MealHistoryChangesDto
{
    public List<MealHistoryItemDto> ToAdd { get; set; } = new();
    public List<MealHistoryItemDto> ToDelete { get; set; } = new();
}