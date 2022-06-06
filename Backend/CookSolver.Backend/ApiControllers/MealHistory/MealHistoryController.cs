using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookSolver.ApiModel;
using CookSolver.Data;
using CookSolver.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookSolver.ApiControllers.MealHistory;

[ApiController]
[Route("[Controller]")]
public class MealHistoryController : ControllerBase
{
    #region Constructor and dependensies
    
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public MealHistoryController(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    #endregion

    [HttpGet]
    public ActionResult<IQueryable<ApiMealHistoryItem>> Get(DateTime? date)
    {
        var dateOnly = date is null
            ? null as DateOnly?
            : DateOnly.FromDateTime(date.Value);
        
        var mealHistoryItems = _dbContext.Set<MealHistoryItem>()
            .Where(x => date == null || x.Date == dateOnly)
            .ProjectTo<ApiMealHistoryItem>(_mapper.ConfigurationProvider);

        return Ok(mealHistoryItems);
    }

    [HttpPost("Changes")]
    public async Task<ActionResult<IEnumerable<ApiMealHistoryItem>>> Post(MealHistoryChangesDto changes)
    {
        var itemsToAdd = _mapper.Map<List<MealHistoryItem>>(changes.ToAdd);
        var itemsToDelete = _mapper.Map<List<MealHistoryItem>>(changes.ToDelete);

        var dates = itemsToAdd
            .Select(x => x.Date)
            .Union(itemsToDelete.Select(x => x.Date));

        var mealIds = itemsToAdd
            .Select(x => x.MealId)
            .Union(itemsToDelete.Select(x => x.MealId));

        var itemsFromDb = await _dbContext.Set<MealHistoryItem>()
            .Where(x => dates.Contains(x.Date) 
                        && mealIds.Contains(x.MealId))
            .ToListAsync();

        itemsToAdd = itemsToAdd
            .GroupJoin(itemsFromDb,
                l => (l.Date, l.MealId),
                r => (r.Date, r.MealId),
                (l, r) => (ItemToAdd: l, AlreadyExist: r.Any()))
            .Where(x => !x.AlreadyExist)
            .Select(x => x.ItemToAdd)
            .ToList();

        itemsToDelete = itemsToDelete
            .GroupJoin(itemsFromDb,
                l => (l.Date, l.MealId),
                r => (r.Date, r.MealId),
                (l, r) => (ItemToAdd: l, Exist: r.Any()))
            .Where(x => x.Exist)
            .Select(x => x.ItemToAdd)
            .ToList();
        
        _dbContext.AddRange(itemsToAdd);
        _dbContext.RemoveRange(itemsToDelete);
        await _dbContext.SaveChangesAsync();

        return Ok(_mapper.Map<IEnumerable<ApiMealHistoryItem>>(itemsToAdd));
    }
}