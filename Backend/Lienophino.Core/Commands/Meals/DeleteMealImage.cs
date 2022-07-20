using Blobs;
using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Lienophino.Core.Commands.Meals;

public class DeleteMealImage : IRequest<DeleteMealImage.Response>
{
    public Guid MealId { get; set; }

    public class Response
    {
        public Meal Meal { get; set; }
    }

    public class Handler : IRequestHandler<DeleteMealImage, Response>
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

        public async Task<Response> Handle(DeleteMealImage request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Set<Meal>()
                .FirstOrDefaultAsync(x => x.Id == request.MealId, cancellationToken);

            if (meal is null)
                throw new Exception("The meal was not found");

            if (meal.ImageId is null)
                throw new Exception("The meal hasn't an image");

            try
            {
                await _blobStorage.Remove(meal.ImageId.Value, cancellationToken);
            }
            catch (CodedException<IBlobStorage.ErrorCodes> e) when (e.Code == IBlobStorage.ErrorCodes.BlobNotFound)
            {
                throw new Exception("The meal's image was not found", e);
            }

            meal.ImageId = null;
            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return new Response {Meal = meal};
        }
    }
}