using System.ComponentModel.DataAnnotations;
using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.MealTags;

public class CreateMealTag: IRequest<MealTag>
{
    [Required]
    public string Name { get; set; }

    public class Handler : IRequestHandler<CreateMealTag, MealTag>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
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