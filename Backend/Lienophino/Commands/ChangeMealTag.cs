using System.ComponentModel.DataAnnotations;
using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Commands;

public class ChangeMealTag: IRequest<MealTag>
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    
    public class Handler: IRequestHandler<ChangeMealTag, MealTag>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<MealTag> Handle(ChangeMealTag request, CancellationToken cancellationToken)
        {
            var mealTag = await _dbContext.Set<MealTag>()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (mealTag is null)
                throw new Exception("Meal tag not found");

            mealTag.Name = request.Name;

            _dbContext.Update(mealTag);
            await _dbContext.SaveChangesAsync();

            return mealTag;
        }
    }
}