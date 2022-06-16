using System.ComponentModel.DataAnnotations;
using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Commands;

public class ChangeMeal: IRequest<Meal>
{
    [Required] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    
    public List<Guid> MealTagIds { get; set; }
    
    public class Handler: IRequestHandler<ChangeMeal, Meal>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        public async Task<Meal> Handle(ChangeMeal request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Set<Meal>()
                .Include(x => x.Meal2MealTags)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (meal is null)
                throw new Exception("Meal not found");

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
            
            meal.Name = request.Name;
            meal.Description = request.Description;
            meal.Meal2MealTags = request.MealTagIds.Select(x => new Meal2MealTag
            {
                MealId = meal.Id,
                MealTagId = x
            }).ToList();

            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}