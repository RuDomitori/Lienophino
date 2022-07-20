using System.ComponentModel.DataAnnotations;
using Blobs;
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
        private readonly IBlobStorage _blobStorage;

        public Handler(DbContext dbContext, IBlobStorage blobStorage)
        {
            _dbContext = dbContext;
            _blobStorage = blobStorage;
        }
        
        #endregion
        
        public async Task<Meal> Handle(DeleteMeal request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Set<Meal>()
                .Include(x => x.Meal2MealTags)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (meal is null)
                throw new Exception("The meal was not found");

            if (meal.ImageId is not null)
                await _blobStorage.RemoveIfExist(meal.ImageId.Value, cancellationToken);
            
            _dbContext.Remove(meal);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return meal;
        }
    }
}