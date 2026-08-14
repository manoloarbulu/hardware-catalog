using HardwareCatalog.Application.Commands;
using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HardwareCatalog.WebApi.Controllers;

/// <summary>
/// API controller for Computer management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ComputersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComputersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all computers.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ComputerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ComputerDto>>> GetAll()
    {
        var computers = await _mediator.Send(new GetAllComputersQuery());
        return Ok(computers);
    }

    /// <summary>
    /// Get a computer by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ComputerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComputerDto>> GetById(Guid id)
    {
        var computer = await _mediator.Send(new GetComputerByIdQuery { Id = id });
        if (computer == null)
            return NotFound();

        return Ok(computer);
    }

    /// <summary>
    /// Create a new computer.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ComputerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ComputerDto>> Create([FromBody] CreateComputerCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing computer.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ComputerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ComputerDto>> Update(Guid id, [FromBody] UpdateComputerCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a computer by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteComputerCommand { Id = id });
        if (!result)
            return NotFound();

        return NoContent();
    }
}
