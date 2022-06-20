using Lienophino.Data;
using Lienophino.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lienophino.Commands;

public class CreateIngredient: IRequest<Ingredient>
{
    public string Name { get; set; }
    
    public class Handler: IRequestHandler<CreateIngredient, Ingredient>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<Ingredient> Handle(CreateIngredient request, CancellationToken cancellationToken)
        {
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            _dbContext.Add(ingredient);
            await _dbContext.SaveChangesAsync();

            return ingredient;
        }
    }
}