using Logitar.Portal.Contracts.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application;
using PokeGame.Application.Moves.Commands;
using PokeGame.Application.Moves.Queries;
using PokeGame.Constants;
using PokeGame.Contracts.Moves;
using PokeGame.Extensions;
using PokeGame.Models.Moves;

namespace PokeGame.Controllers;

[ApiController]
[Authorize(Policy = Policies.Gamemaster)]
[Route("moves")]
public class MoveController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public MoveController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<Move>> CreateAsync([FromBody] CreateMovePayload payload, CancellationToken cancellationToken)
  {
    Move move = await _pipeline.ExecuteAsync(new CreateMoveCommand(payload), cancellationToken);
    Uri location = HttpContext.BuildLocation("moves", new Dictionary<string, string>
    {
      ["id"] = move.Id.ToString()
    });
    return Created(location, move);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Move>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    Move? move = await _pipeline.ExecuteAsync(new ReadMoveQuery(id, UniqueName: null), cancellationToken);
    return move == null ? NotFound() : Ok(move);
  }

  [HttpGet("unique-name:{uniqueName}")]
  public async Task<ActionResult<Move>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    Move? move = await _pipeline.ExecuteAsync(new ReadMoveQuery(Id: null, uniqueName), cancellationToken);
    return move == null ? NotFound() : Ok(move);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Move>> ReplaceAsync(Guid id, [FromBody] ReplaceMovePayload payload, long? version, CancellationToken cancellationToken = default)
  {
    Move? move = await _pipeline.ExecuteAsync(new ReplaceMoveCommand(id, payload, version), cancellationToken);
    return move == null ? NotFound() : Ok(move);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<Move>>> SearchAsync([FromQuery] SearchMovesParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<Move> moves = await _pipeline.ExecuteAsync(new SearchMovesQuery(parameters.ToPayload()), cancellationToken);
    return Ok(moves);
  }
}
