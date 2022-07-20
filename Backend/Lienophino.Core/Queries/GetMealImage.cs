using Blobs;
using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Queries;

public sealed class GetMealImage: IRequest<GetMealImage.Response>
{
    public Guid MealId { get; set; }
    
    public sealed class Response
    {
        public Meal Meal { get; set; }
        public Stream Stream { get; set; }
    }
    
    public class Handler : IRequestHandler<GetMealImage, Response>
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

        public async Task<Response> Handle(GetMealImage request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Set<Meal>()
                .FirstOrDefaultAsync(x => x.Id == request.MealId, cancellationToken);

            if (meal is null)
                throw new Exception("The meal was not found");

            if (meal.ImageId is null)
                throw new Exception("The meal hasn't an image");

            var stream = await _blobStorage.OpenRead(meal.ImageId.Value, cancellationToken);

            return new Response
            {
                Meal = meal,
                Stream = stream
            };
        }
    }
}