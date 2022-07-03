using System.ComponentModel.DataAnnotations;
using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.Meals;

public class ChangeMeal: IRequest<Meal>
{
    [Required] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    
    public List<Guid> MealTagIds { get; set; }
    public List<Guid> IngredientIds { get; set; }
    
    public class Handler: IRequestHandler<ChangeMeal, Meal>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        public async Task<Meal> Handle(ChangeMeal request, CancellationToken cancellationToken)
        {
            #region Загрузка Meal
            
            var meal = await _dbContext.Set<Meal>()
                .Include(x => x.Meal2MealTags)
                .Include(x => x.Meal2Ingredients)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (meal is null)
                throw new Exception("Meal not found");

            #endregion

            #region Загрузка MealTag
            
            var mealTagsFromDb = await _dbContext.Set<MealTag>()
                .Where(x => request.MealTagIds.Contains(x.Id))
                .ToListAsync();

            var notExistedTagCount = request.MealTagIds
                .GroupJoin(mealTagsFromDb,
                    l => l,
                    r => r.Id,
                    (l, r) => (MealTagId: l, Exist: r.Any()))
                .Count(x => !x.Exist);

            if (notExistedTagCount > 0)
                throw new Exception($"{notExistedTagCount} meal tags not found");

            #endregion

            #region Загрузка Ingredients
            
            var ingredientsFromDb = await _dbContext.Set<Ingredient>()
                .Where(x => request.IngredientIds.Contains(x.Id))
                .ToListAsync();

            var notExistedIngredientCount = request.IngredientIds
                .GroupJoin(ingredientsFromDb,
                    l => l,
                    r => r.Id,
                    (_, r) => r.Any())
                .Count(x => !x);

            if (notExistedIngredientCount > 0)
                throw new Exception($"{notExistedIngredientCount} ingredients not found");

            #endregion
            
            meal.Name = request.Name;
            meal.Description = request.Description;
            
            meal.Meal2MealTags = request.MealTagIds.Select(x => new Meal2MealTag
            {
                MealId = meal.Id,
                MealTagId = x
            }).ToList();
            
            meal.Meal2Ingredients = request.IngredientIds.Select(x => new Meal2Ingredient
            {
                MealId = meal.Id,
                IngredientId = x
            }).ToList();

            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}