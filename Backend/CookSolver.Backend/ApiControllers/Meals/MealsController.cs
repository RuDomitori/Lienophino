using AutoMapper;
using CookSolver.ApiModel;
using CookSolver.Commands;
using CookSolver.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CookSolver.ApiControllers.Meals;

[ApiController]
[Route("[controller]")]
public class MealsController : ControllerBase
{
    #region Constructor and dependensies
    
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public MealsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    #endregion
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiMeal>>> Get()
    {
        var meals = await _mediator.Send(new GetMeals());

        return Ok(_mapper.Map<IEnumerable<ApiMeal>>(meals));
    }

    public class PostDto
    {
        [BindRequired] public string Name { get; set; }
        public string Description { get; set; }
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiMeal>> Post(PostDto dto)
    {
        var meal = await _mediator.Send(new CreateMeal
        {
            Name = dto.Name,
            Description = dto.Description
        });
        
        return Ok(_mapper.Map<ApiMeal>(meal));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiMeal>> Put(ApiMeal dto)
    {
        var meal = await _mediator.Send(new ChangeMeal
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        });

        return Ok(_mapper.Map<ApiMeal>(meal));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var meal = await _mediator.Send(new DeleteMeal{Id = id});
        
        return Ok(_mapper.Map<ApiMeal>(meal));
    }
}