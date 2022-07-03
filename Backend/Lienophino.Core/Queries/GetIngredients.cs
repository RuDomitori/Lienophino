using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Queries;

public class GetIngredients: IRequest<List<Ingredient>>
{
    public class Handler: IRequestHandler<GetIngredients, List<Ingredient>>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<List<Ingredient>> Handle(GetIngredients request, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Ingredient>().ToListAsync();
        }
    }
}