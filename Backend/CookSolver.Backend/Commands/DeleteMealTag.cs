using CookSolver.Data;
using CookSolver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.Commands;

public class DeleteMealTag: IRequest<MealTag>
{
    public Guid Id { get; set; }
    
    public class Handler: IRequestHandler<DeleteMealTag, MealTag>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<MealTag> Handle(DeleteMealTag request, CancellationToken cancellationToken)
        {
            
            var mealTag = await _dbContext.Set<MealTag>()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (mealTag is null)
                throw new Exception("Meal tag not found");

            _dbContext.Remove(mealTag);
            await _dbContext.SaveChangesAsync();

            return mealTag;
        }
    }
}