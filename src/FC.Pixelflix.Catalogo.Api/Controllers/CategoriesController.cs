using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FC.Pixelflix.Catalogo.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryModelResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var response =  await _mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Create), new {response.Id, response});
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response =  await _mediator.Send(new GetCategoryRequest(id), cancellationToken);
        return Ok(response);
    }
}