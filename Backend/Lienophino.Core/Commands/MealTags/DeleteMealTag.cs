using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.MealTags;

public class DeleteMealTag: IRequest<MealTag>
{
    public Guid Id { get; set; }
    
    public class Handler: IRequestHandler<DeleteMealTag, MealTag>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
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