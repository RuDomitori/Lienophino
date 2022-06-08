using System.ComponentModel.DataAnnotations;
using CookSolver.Data;
using CookSolver.Data.Entities;
using MediatR;

namespace CookSolver.Commands;

public class CreateMealTag: IRequest<MealTag>
{
    [Required]
    public string Name { get; set; }

    public class Handler : IRequestHandler<CreateMealTag, MealTag>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<MealTag> Handle(CreateMealTag request, CancellationToken cancellationToken)
        {
            var mealTag = new MealTag
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            _dbContext.Add(mealTag);
            await _dbContext.SaveChangesAsync();
            
            return mealTag;
        }
    }
}