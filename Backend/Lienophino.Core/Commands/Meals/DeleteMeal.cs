using System.ComponentModel.DataAnnotations;
using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.Meals;

public class DeleteMeal: IRequest<Meal>
{
    [Required] public Guid Id { get; set; }

    public class Handler : IRequestHandler<DeleteMeal, Meal>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        public async Task<Meal> Handle(DeleteMeal request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Set<Meal>()
                .Include(x => x.Meal2MealTags)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (meal is null)
                throw new Exception("Meal not found");

            _dbContext.Remove(meal);
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}