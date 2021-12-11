using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Queries.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HepsiFlyCaseStudy.Controllers;

[Route("/api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? name = null)
    {
        var requestModel = new ListCategoriesQueryRequest
        {
            Name = name
        };

        var result = await _mediator.Send(requestModel);
        if (result == null || !result.Any())
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var requestModel = new GetCategoryQueryRequest
        {
            Id = id
        };

        var result = await _mediator.Send(requestModel);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCategoryCommandRequest requestModel)
    {
        var result = await _mediator.Send(requestModel);

        return StatusCode(201, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put([FromBody] UpdateCategoryCommandRequest requestModel, [FromRoute] Guid id)
    {
        requestModel.Id = id;

        var result = await _mediator.Send(requestModel);
        if (result == null)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var requestModel = new DeleteCategoryCommandRequest
        {
            Id = id
        };

        var result = await _mediator.Send(requestModel);

        if (result == null)
            return NotFound();

        return Ok();
    }
}