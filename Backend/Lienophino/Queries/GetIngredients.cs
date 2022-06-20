using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Queries;

public class GetIngredients: IRequest<List<Ingredient>>
{
    public class Handler: IRequestHandler<GetIngredients, List<Ingredient>>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
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