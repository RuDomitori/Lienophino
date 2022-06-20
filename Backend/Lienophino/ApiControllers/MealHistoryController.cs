using AutoMapper;
using Lienophino.ApiModel;
using Lienophino.Commands;
using Lienophino.Commands.Meals;
using Lienophino.Data.Entities;
using Lienophino.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lienophino.ApiControllers;

[ApiController]
[Route("[Controller]")]
public class MealHistoryController : ControllerBase
{
    #region Constructor and dependensies

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public MealHistoryController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    #endregion

    [HttpGet]
    public async Task<ActionResult<IQueryable<ApiMealHistoryItem>>> Get(DateTime? date)
    {
        var items = await _mediator.Send(new GetMealHistory
        {
            Date = date is null
                ? null
                : DateOnly.FromDateTime(date.Value)
        });

        return Ok(_mapper.Map<List<ApiMealHistoryItem>>(items));
    }

    public class PostDto
    {
        public List<ApiMealHistoryItem> ToAdd { get; set; } = new();
        public List<ApiMealHistoryItem> ToDelete { get; set; } = new();
    }
    
    [HttpPost("Changes")]
    public async Task<ActionResult<IEnumerable<ApiMealHistoryItem>>> Post(PostDto changes)
    {
        var addedItems = await _mediator.Send(new ChangeMealHistory
        {
            ToAdd = _mapper.Map<List<MealHistoryItem>>(changes.ToAdd),
            ToDelete = _mapper.Map<List<MealHistoryItem>>(changes.ToDelete)
        });

        return Ok(_mapper.Map<IEnumerable<ApiMealHistoryItem>>(addedItems));
    }
}