using System.ComponentModel.DataAnnotations;
using CookSolver.Data;
using CookSolver.Data.Entities;
using MediatR;

namespace CookSolver.Commands;

public class CreateMeal: IRequest<Meal>
{
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    
    public class Handler: IRequestHandler<CreateMeal, Meal>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
        
        public async Task<Meal> Handle(CreateMeal request, CancellationToken cancellationToken)
        {
            var meal = new Meal
            {
                Name = request.Name,
                Description = request.Description
            };

            _dbContext.Add(meal);
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}