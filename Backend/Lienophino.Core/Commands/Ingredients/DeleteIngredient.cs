using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.Ingredients;

public class DeleteIngredient: IRequest<Ingredient>
{
    public Guid Id { get; set; }
    
    public class Handler: IRequestHandler<DeleteIngredient, Ingredient>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<Ingredient> Handle(DeleteIngredient request, CancellationToken cancellationToken)
        {
            var ingredient = await _dbContext.Set<Ingredient>()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (ingredient is null)
                throw new Exception("Ingredient not found");

            _dbContext.Remove(ingredient);
            await _dbContext.SaveChangesAsync();

            return ingredient;
        }
    }
}