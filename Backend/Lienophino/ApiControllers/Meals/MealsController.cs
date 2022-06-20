using AutoMapper;
using Lienophino.ApiModel;
using Lienophino.Commands;
using Lienophino.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lienophino.ApiControllers.Meals;

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
    public async Task<ActionResult<IEnumerable<ApiMeal.WithNavProps>>> Get(bool includeMealTags,
        bool includeIngredients)
    {
        var meals = await _mediator.Send(new GetMeals
        {
            IncludeMealTags = includeMealTags,
            IncludeIngredients = includeIngredients
        });

        return Ok(_mapper.Map<IEnumerable<ApiMeal.WithNavProps>>(meals));
    }

    public class PostDto
    {
        [BindRequired] public string Name { get; set; }
        public string Description { get; set; }

        public List<Guid> MealTagIds { get; set; } = new();
        public List<Guid> IngredientIds { get; set; } = new();
    }

    [HttpPost]
    public async Task<ActionResult<ApiMeal>> Post(PostDto dto)
    {
        var meal = await _mediator.Send(new CreateMeal
        {
            Name = dto.Name,
            Description = dto.Description,
            MealTagIds = dto.MealTagIds,
            IngredientIds = dto.IngredientIds
        });

        return Ok(_mapper.Map<ApiMeal>(meal));
    }

    public class PutDto
    {
        [BindRequired] public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> MealTagIds { get; set; } = new();
        public List<Guid> IngredientIds { get; set; } = new();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiMeal>> Put(Guid id, PutDto dto)
    {
        var meal = await _mediator.Send(new ChangeMeal
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            MealTagIds = dto.MealTagIds,
            IngredientIds = dto.IngredientIds
        });

        return Ok(_mapper.Map<ApiMeal>(meal));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var meal = await _mediator.Send(new DeleteMeal {Id = id});

        return Ok(_mapper.Map<ApiMeal>(meal));
    }
}