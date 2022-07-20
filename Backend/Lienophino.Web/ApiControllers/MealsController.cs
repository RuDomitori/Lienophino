using AutoMapper;
using Lienophino.ApiModel;
using Lienophino.Core.Commands.Meals;
using Lienophino.Core.Entities;
using Lienophino.Core.Queries;
using Lienophino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace Lienophino.ApiControllers;

[ApiController]
[Route("[controller]")]
public class MealsController : ControllerBase
{
    #region Constructor and dependensies

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly AppDbContext _dbContext;

    public MealsController(IMapper mapper, IMediator mediator, AppDbContext dbContext)
    {
        _mapper = mapper;
        _mediator = mediator;
        _dbContext = dbContext;
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

    [HttpPatch("{mealId:guid}/Image")]
    [Consumes("multipart/form-data", "application/json")]
    public async Task UploadImage(Guid mealId, [FromForm, BindRequired] IFormFile formFile)
    {
        await _mediator.Send(new UploadMealImage
        {
            MealId = mealId,
            Stream = formFile.OpenReadStream()
        });
    }

    [HttpGet("{mealId:guid}/Image")]
    public async Task<FileStreamResult> GetImage(Guid mealId)
    {
        var response = await _mediator.Send(new GetMealImage{MealId = mealId});

        return File(response.Stream, "image/*", response.Meal.Name);
    }

    [HttpDelete("{mealId:guid}/Image")]
    public async Task DeleteImage(Guid mealId)
    {
        await _mediator.Send(new DeleteMealImage {MealId = mealId});
    }

    [HttpGet("BestChoice")]
    public async Task<ActionResult<IEnumerable<ApiMeal>>> BestChoice()
    {
        var mealHistoryItems = await _dbContext.Set<MealHistoryItem>()
            .ToListAsync();

        var meals = await _dbContext.Set<Meal>()
            .Include(x => x.Meal2MealTags)
            .ThenInclude(x => x.MealTag)
            .Include(x => x.Meal2Ingredients)
            .ThenInclude(x => x.Ingredient)
            .ToListAsync();

        // Писать туть

        return Ok(_mapper.Map<IEnumerable<ApiMeal>>(meals));
    }
}