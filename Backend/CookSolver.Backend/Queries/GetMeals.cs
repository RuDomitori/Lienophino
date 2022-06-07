using CookSolver.Data;
using CookSolver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.Queries;

public class GetMeals: IRequest<List<Meal>>
{
    public class Handler : IRequestHandler<GetMeals, List<Meal>>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<List<Meal>> Handle(GetMeals request, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Meal>().ToListAsync();
        }
    }
}