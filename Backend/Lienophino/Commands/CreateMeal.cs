using System.ComponentModel.DataAnnotations;
using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Commands;

public class CreateMeal: IRequest<Meal>
{
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    public List<Guid> MealTagIds { get; set; }
    
    public class Handler: IRequestHandler<CreateMeal, Meal>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<Meal> Handle(CreateMeal request, CancellationToken cancellationToken)
        {
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

            var meal = new Meal
            {
                Name = request.Name,
                Description = request.Description
            };
            
            _dbContext.Add(meal);
            _dbContext.AddRange(request.MealTagIds.Select(x => new Meal2MealTag
            {
                MealTagId = x,
                MealId = meal.Id
            }));
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}