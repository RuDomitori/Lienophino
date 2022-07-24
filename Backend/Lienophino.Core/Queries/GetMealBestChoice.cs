using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Lienophino.Core.Queries;

public sealed class GetMealBestChoice : IRequest<List<Meal>>
{
    public sealed class Handler : IRequestHandler<GetMealBestChoice, List<Meal>>
    {
        #region Constructor and dependencies

        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        public async Task<List<Meal>> Handle(GetMealBestChoice request, CancellationToken cancellationToken)
        {
            // TODO: Resolve timezone edge-cases
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            var mealHistoryItems = await _dbContext.Set<MealHistoryItem>()
                .AsNoTracking()
                .OrderBy(x => x.Date)
                .Where(x => x.Date <= today)
                .ToListAsync(cancellationToken);

            var meals = await _dbContext.Set<Meal>()
                .AsNoTracking()
                .Include(x => x.Meal2MealTags)
                .ThenInclude(x => x.MealTag)
                .Include(x => x.Meal2Ingredients)
                .ThenInclude(x => x.Ingredient)
                .ToListAsync(cancellationToken);

            mealHistoryItems
                .Join(
                    meals,
                    l => l.MealId,
                    r => r.Id,
                    (l, r) => (l, r)
                )
                .ForEach(x => x.l.Meal = x.r);

            var diffs = new List<(Meal meal, double value)>(meals.Count);
            foreach (var meal in meals)
            {
                double mealDiffSum = 0;
                foreach (var historyItem in mealHistoryItems)
                {
                    double tagSetsIntersectionValue =
                        historyItem.Meal.Meal2MealTags.Select(x => x.MealTagId)
                            .Intersect(meal.Meal2MealTags.Select(x => x.MealTagId))
                            .Count();

                    double tagSetsUnionValue =
                        historyItem.Meal.Meal2MealTags.Select(x => x.MealTagId)
                            .Union(meal.Meal2MealTags.Select(x => x.MealTagId))
                            .Count();

                    var tagSetsDifferenceValue = tagSetsUnionValue - tagSetsIntersectionValue;
                    var tagSetsDifferenceQuotient = tagSetsDifferenceValue / tagSetsUnionValue;


                    double ingredientSetsIntersectionValue =
                        historyItem.Meal.Meal2Ingredients.Select(x => x.IngredientId)
                            .Intersect(meal.Meal2Ingredients.Select(x => x.IngredientId))
                            .Count();

                    double ingredientSetsUnionValue =
                        historyItem.Meal.Meal2Ingredients.Select(x => x.IngredientId)
                            .Union(meal.Meal2Ingredients.Select(x => x.IngredientId))
                            .Count();

                    var ingredientSetsDifferenceValue = ingredientSetsUnionValue - ingredientSetsIntersectionValue;
                    var ingredientSetsDifferenceQuotient = ingredientSetsDifferenceValue / ingredientSetsUnionValue;

                    var dateDiff = today.DayNumber - historyItem.Date.DayNumber;

                    mealDiffSum += (ingredientSetsDifferenceQuotient + tagSetsDifferenceQuotient) / (dateDiff + 1);
                }

                diffs.Add((meal, mealDiffSum));
            }

            var orderedMeals = diffs
                .OrderByDescending(x => x.value)
                .Select(x => x.meal)
                .ToList();

            return orderedMeals;
        }
    }
}