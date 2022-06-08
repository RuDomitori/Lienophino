using CookSolver.Data;
using CookSolver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.Queries;

public class GetMealTags: IRequest<List<MealTag>>
{
    public class Handler: IRequestHandler<GetMealTags, List<MealTag>>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
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