using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Queries;

public class GetMealTags: IRequest<List<MealTag>>
{
    public class Handler: IRequestHandler<GetMealTags, List<MealTag>>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<List<MealTag>> Handle(GetMealTags request, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<MealTag>().ToListAsync();
        }
    }
}