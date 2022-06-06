using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookSolver.ApiModel;
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
    
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public MealsController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    #endregion
    
    [HttpGet]
    public ActionResult<IEnumerable<ApiMeal>> Get()
    {
        return Ok(_dbContext.Set<Meal>().ProjectTo<ApiMeal>(_mapper.ConfigurationProvider));
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