using AutoMapper;
using Lienophino.ApiModel;
using Lienophino.Core.Commands.Ingredients;
using Lienophino.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lienophino.ApiControllers;

[ApiController]
[Route("[controller]")]
public class IngredientsController: ControllerBase
{
    #region Constructor and dependencies

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IngredientsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    #endregion

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiIngredient>>> Get()
    {
        var ingredients = await _mediator.Send(new GetIngredients());

        return Ok(_mapper.Map<IEnumerable<ApiIngredient>>(ingredients));
    }
    
    public class PostDto
    {
        public string Name { get; set; }
    }

    [HttpPost]
    public async Task<ActionResult<ApiIngredient>> Post(PostDto dto)
    {
        var ingredient = await _mediator.Send(new CreateIngredient{Name = dto.Name});

        return Ok(_mapper.Map<ApiIngredient>(ingredient));
    }
    
    public class PutDto
    {
        public string Name { get; set; }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiIngredient>> Put(Guid id, [FromBody] PutDto dto)
    {
        var ingredient = await _mediator.Send(new ChangeIngredient
        {
            Id = id,
            Name = dto.Name
        });

        return Ok(_mapper.Map<ApiIngredient>(ingredient));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiIngredient>> Delete(Guid id)
    {
        var ingredient = await _mediator.Send(new DeleteIngredient{Id = id});

        return Ok(_mapper.Map<ApiIngredient>(ingredient));
    }
}