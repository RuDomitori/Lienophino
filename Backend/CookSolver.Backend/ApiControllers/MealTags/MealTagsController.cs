using AutoMapper;
using CookSolver.ApiModel;
using CookSolver.Commands;
using CookSolver.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CookSolver.ApiControllers.MealTags;

[ApiController]
[Route("[Controller]")]
public class MealTagsController: ControllerBase
{
    #region Constructor and dependencies

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public MealTagsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    #endregion

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiMealTag>>> Get()
    {
        var mealTags = await _mediator.Send(new GetMealTags());

        return Ok(_mapper.Map<IEnumerable<ApiMealTag>>(mealTags));
    }


    public class PostDto
    {
        [BindRequired] public string Name { get; set; }
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiMealTag>> Post(PostDto dto)
    {
        var mealTag = await _mediator.Send(new CreateMealTag
        {
            Name = dto.Name
        });

        return Ok(_mapper.Map<ApiMealTag>(mealTag));
    }
    
    
    public class PutDto
    {
        [BindRequired] public string Name { get; set; }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiMealTag>> Post(Guid id, [FromBody] PutDto dto)
    {
        var mealTag = await _mediator.Send(new ChangeMealTag
        {
            Id = id,
            Name = dto.Name
        });

        return Ok(_mapper.Map<ApiMealTag>(mealTag));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiMealTag>> Post(Guid id)
    {
        var mealTag = await _mediator.Send(new DeleteMealTag
        {
            Id = id
        });

        return Ok(_mapper.Map<ApiMealTag>(mealTag));
    }
}