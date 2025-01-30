using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Moves.Commands;
using PokeGame.Application.Moves.Models;
using PokeGame.Application.Moves.Queries;
using PokeGame.Models.Move;

namespace PokeGame.Controllers;

[ApiController]
[Authorize]
[Route("moves")]
public class MoveController : ControllerBase
{
  private readonly IMediator _mediator;

  public MoveController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<MoveModel>> CreateAsync([FromBody] CreateOrReplaceMovePayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceMoveCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceMoveResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<MoveModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadMoveQuery command = new(id, UniqueName: null);
    MoveModel? move = await _mediator.Send(command, cancellationToken);
    return move == null ? NotFound() : Ok(move);
  }

  [HttpGet("name:{uniqueName}")]
  public async Task<ActionResult<MoveModel>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    ReadMoveQuery command = new(Id: null, uniqueName);
    MoveModel? move = await _mediator.Send(command, cancellationToken);
    return move == null ? NotFound() : Ok(move);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<MoveModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceMovePayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceMoveCommand command = new(id, payload, version);
    CreateOrReplaceMoveResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<MoveModel>>> SearchAsync([FromQuery] SearchMovesParameters parameters, CancellationToken cancellationToken)
  {
    SearchMovesPayload payload = parameters.ToPayload();
    SearchMovesQuery query = new(payload);
    SearchResults<MoveModel> moves = await _mediator.Send(query, cancellationToken);
    return Ok(moves);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<MoveModel>> UpdateAsync(Guid id, [FromBody] UpdateMovePayload payload, CancellationToken cancellationToken)
  {
    UpdateMoveCommand command = new(id, payload);
    MoveModel? move = await _mediator.Send(command, cancellationToken);
    return move == null ? NotFound() : Ok(move);
  }

  private ActionResult ToActionResult(CreateOrReplaceMoveResult result)
  {
    if (result.Move == null)
    {
      return NotFound();
    }
    else if (result.Created)
    {
      Uri location = new($"{Request.Scheme}://{Request.Host}/moves/{result.Move.Id}", UriKind.Absolute);
      return Created(location, result.Move);
    }

    return Ok(result.Move);
  }
}
