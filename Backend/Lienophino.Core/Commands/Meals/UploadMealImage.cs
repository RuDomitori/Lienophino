using Blobs;
using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.Meals;

public class UploadMealImage : IRequest<UploadMealImage.Response>
{
    public Guid MealId { get; set; }
    public Stream Stream { get; set; }

    public class Response
    {
    }

    public class Handler : IRequestHandler<UploadMealImage, Response>
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

        public async Task<Response> Handle(UploadMealImage request, CancellationToken cancellationToken)
        {
            await using var stream = request.Stream;
            
            var meal = await _dbContext.Set<Meal>()
                .FirstOrDefaultAsync(x => x.Id == request.MealId, cancellationToken);

            if (meal is null)
                throw new Exception("Meal was not found");

            var imageId = meal.ImageId ?? Guid.NewGuid();
            await _blobStorage.CreateOrReplace(imageId, stream, cancellationToken);

            if (meal.ImageId is null)
            {
                meal.ImageId = imageId;
                _dbContext.Update(meal);
                await _dbContext.SaveChangesAsync(CancellationToken.None);
            }

            return new Response();
        }
    }
}