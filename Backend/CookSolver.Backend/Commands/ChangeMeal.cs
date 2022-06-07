using System.ComponentModel.DataAnnotations;
using CookSolver.Data;
using CookSolver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.Commands;

public class ChangeMeal: IRequest<Meal>
{
    [Required] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    
    public class Handler: IRequestHandler<ChangeMeal, Meal>
    {
        #region Constructor and dependencies
        
        private readonly AppDbContext _dbContext;

        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #endregion
        
        public async Task<Meal> Handle(ChangeMeal request, CancellationToken cancellationToken)
        {
            var meal = await _dbContext.Set<Meal>()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (meal is null)
                throw new Exception("Meal not found");

            meal.Name = request.Name;
            meal.Description = request.Description;

            _dbContext.Update(meal);
            await _dbContext.SaveChangesAsync();

            return meal;
        }
    }
}