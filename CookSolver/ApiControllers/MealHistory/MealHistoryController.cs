﻿using CookSolver.Data;
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

    public MealHistoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MealHistoryItem>>> Get(DateTime? date)
    {
        var dateOnly = date is null
            ? null as DateOnly?
            : DateOnly.FromDateTime(date.Value);
        
        var mealHistoryItems = await _dbContext.Set<MealHistoryItem>()
            .Where(x => date == null || x.Date == dateOnly)
            .ToListAsync();

        return Ok(mealHistoryItems.Select(x => new MealHistoryItemDto
        {
            Date = x.Date.ToDateTime(TimeOnly.MinValue),
            MealId = x.MealId
        }));
    }

    [HttpPost("Changes")]
    public async Task<ActionResult<IEnumerable<MealHistoryItem>>> Post(MealHistoryChangesDto changes)
    {
        var itemsToAdd = changes.ToAdd
            .Select(x => new MealHistoryItem
            {
                Date = DateOnly.FromDateTime(x.Date),
                MealId = x.MealId
            })
            .ToList();
        
        var itemsToDelete = changes.ToDelete
            .Select(x => new MealHistoryItem
            {
                Date = DateOnly.FromDateTime(x.Date),
                MealId = x.MealId
            })
            .ToList();

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

        return Ok(itemsToAdd.Select(x => new MealHistoryItemDto
        {
            Date = x.Date.ToDateTime(TimeOnly.MinValue),
            MealId = x.MealId
        }));
    }
}