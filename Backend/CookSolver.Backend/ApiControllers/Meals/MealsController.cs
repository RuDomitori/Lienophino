using CookSolver.Data;
using CookSolver.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.ApiControllers.Meals;

[ApiController]
[Route("[controller]")]
public class MealsController : ControllerBase
{
    #region Constructor and dependensies
    
    private readonly ILogger<MealsController> _logger;
    private readonly AppDbContext _dbContext;

    public MealsController(ILogger<MealsController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    #endregion
    
    [HttpGet]
    public ActionResult<IEnumerable<Meal>> Get()
    {
        return Ok(_dbContext.Set<Meal>());
    }

    [HttpPost]
    public async Task<ActionResult<Meal>> Post(Meal meal)
    {
        meal.Id = Guid.NewGuid();
        _dbContext.Add(meal);
        await _dbContext.SaveChangesAsync();
        return Ok(meal);
    }
    
    [HttpPut]
    public async Task<ActionResult<Meal>> Put(Meal meal)
    {
        _dbContext.Update(meal);
        await _dbContext.SaveChangesAsync();
        return Ok(meal);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var meal = await _dbContext.Set<Meal>()
            .SingleAsync(x => x.Id == id);
        _dbContext.Remove(meal);
        await _dbContext.SaveChangesAsync();
        return Ok(id);
    }
}