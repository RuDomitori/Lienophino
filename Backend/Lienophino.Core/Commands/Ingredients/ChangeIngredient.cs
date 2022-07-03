using Lienophino.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Core.Commands.Ingredients;

public class ChangeIngredient: IRequest<Ingredient>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public class Handler: IRequestHandler<ChangeIngredient, Ingredient>
    {
        #region Constructor and dependencies
        
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<Ingredient> Handle(ChangeIngredient request, CancellationToken cancellationToken)
        {
            var ingredient = await _dbContext.Set<Ingredient>()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if (ingredient is null)
                throw new Exception("Ingredient not found");

            ingredient.Name = request.Name;

            _dbContext.Update(ingredient);
            await _dbContext.SaveChangesAsync();

            return ingredient;
        }
    }
}