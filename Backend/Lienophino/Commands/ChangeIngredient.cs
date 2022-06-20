using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Commands;

public class ChangeIngredient: IRequest<Ingredient>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public class Handler: IRequestHandler<ChangeIngredient, Ingredient>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
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