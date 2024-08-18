using FC.Pixelflix.Catalogo.Api.ApiModels.Category;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
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
    [ProducesResponseType(typeof(CategoryModelResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response =  await _mediator.Send(new GetCategoryRequest(id), cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(CategoryModelResponse),StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
        )
    {
        var request = new ListCategoriesRequest();
        if(page is not null) request.Page = page.Value;
        if(perPage is not null) request.PerPage = perPage.Value;
        if(!String.IsNullOrWhiteSpace(search)) request.Search = search;
        if(!String.IsNullOrWhiteSpace(sort)) request.Sort = sort;
        if(dir is not null) request.Dir = dir.Value;
        
        var response =  await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryRequest(id), cancellationToken);
        return NoContent();
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryModelResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateCategoryApiRequest request, CancellationToken cancellationToken)
    {
        var input = new UpdateCategoryRequest(id, request.Name, request.Description, request.IsActive);
        var response =  await _mediator.Send(input, cancellationToken);
        return Ok(response);
    }
}
