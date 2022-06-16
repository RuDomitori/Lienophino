using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Commands;

public class ChangeMealHistory: IRequest<List<MealHistoryItem>>
{
    public List<MealHistoryItem> ToAdd { get; set; }
    public List<MealHistoryItem> ToDelete { get; set; }

    public class Handler : IRequestHandler<ChangeMealHistory, List<MealHistoryItem>>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        public async Task<List<MealHistoryItem>> Handle(ChangeMealHistory request, CancellationToken cancellationToken)
        {
            var dates = request.ToAdd
                .Select(x => x.Date)
                .Union(request.ToDelete.Select(x => x.Date));

            var mealIds = request.ToAdd
                .Select(x => x.MealId)
                .Union(request.ToDelete.Select(x => x.MealId));

            var itemsFromDb = await _dbContext.Set<MealHistoryItem>()
                .Where(x => dates.Contains(x.Date) 
                            && mealIds.Contains(x.MealId))
                .ToListAsync();

            var itemsToAdd = request.ToAdd
                .GroupJoin(itemsFromDb,
                    l => (l.Date, l.MealId),
                    r => (r.Date, r.MealId),
                    (l, r) => (ItemToAdd: l, AlreadyExist: r.Any()))
                .Where(x => !x.AlreadyExist)
                .Select(x => x.ItemToAdd)
                .ToList();

            var itemsToDelete = request.ToDelete
                .GroupJoin(itemsFromDb,
                    l => (l.Date, l.MealId),
                    r => (r.Date, r.MealId),
                    (l, r) => (ItemToAdd: l, Exist: r.Any()))
                .Where(x => x.Exist)
                .Select(x => x.ItemToAdd)
                .ToList();
        
            _dbContext.AddRange(itemsToAdd);
            _dbContext.RemoveRange(itemsToDelete);
            await _dbContext.SaveChangesAsync();

            return itemsToAdd;
        }
    }
}