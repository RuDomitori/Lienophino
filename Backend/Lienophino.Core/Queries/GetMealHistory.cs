using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Queries;

public class GetMealHistory: IRequest<List<MealHistoryItem>>
{
    public DateOnly? Date { get; set; }
    
    public class Handler : IRequestHandler<GetMealHistory, List<MealHistoryItem>>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<List<MealHistoryItem>> Handle(GetMealHistory request, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<MealHistoryItem>()
                .Where(x => request.Date == null || x.Date == request.Date)
                .ToListAsync();
        }
    }
}