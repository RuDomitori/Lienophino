using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Queries;

public class GetMeals: IRequest<List<Meal>>
{
    public bool IncludeMealTags { get; set; }
    public bool IncludeIngredients { get; set; }

    public class Handler : IRequestHandler<GetMeals, List<Meal>>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<List<Meal>> Handle(GetMeals request, CancellationToken cancellationToken)
        {
            var queryable = _dbContext.Set<Meal>().AsQueryable();

            queryable = request.IncludeMealTags
                ? queryable.Include(x => x.Meal2MealTags).ThenInclude(x => x.MealTag)
                : queryable;
            
            queryable = request.IncludeIngredients
                ? queryable.Include(x => x.Meal2Ingredients).ThenInclude(x => x.Ingredient)
                : queryable;

            return await queryable.ToListAsync();
        }
    }
}