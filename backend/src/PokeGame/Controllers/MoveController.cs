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
[Authorize(Policy = Policies.Administrator)]
[Route("moves")]
public class MoveController : ControllerBase
{
  private readonly IRequestPipeline _pipeline;

  public MoveController(IRequestPipeline pipeline)
  {
    _pipeline = pipeline;
  }

  [HttpPost]
  public async Task<ActionResult<MoveModel>> CreateAsync([FromBody] CreateOrReplaceMovePayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceMoveResult result = await _pipeline.ExecuteAsync(new CreateOrReplaceMoveCommand(Id: null, payload, Version: null), cancellationToken);
    return GetActionResult(result);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<MoveModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
  {
    MoveModel? move = await _pipeline.ExecuteAsync(new DeleteMoveCommand(id), cancellationToken);
    return GetActionResult(move);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<MoveModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    MoveModel? move = await _pipeline.ExecuteAsync(new ReadMoveQuery(id, UniqueName: null), cancellationToken);
    return GetActionResult(move);
  }

  [HttpGet("unique-name:{uniqueName}")]
  public async Task<ActionResult<MoveModel>> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    MoveModel? move = await _pipeline.ExecuteAsync(new ReadMoveQuery(Id: null, uniqueName), cancellationToken);
    return GetActionResult(move);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<MoveModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceMovePayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceMoveResult result = await _pipeline.ExecuteAsync(new CreateOrReplaceMoveCommand(id, payload, version), cancellationToken);
    return GetActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<MoveModel>>> SearchAsync([FromQuery] SearchMovesParameters parameters, CancellationToken cancellationToken)
  {
    SearchResults<MoveModel> moves = await _pipeline.ExecuteAsync(new SearchMovesQuery(parameters.ToPayload()), cancellationToken);
    return Ok(moves);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<MoveModel>> UpdateAsync(Guid id, [FromBody] UpdateMovePayload payload, CancellationToken cancellationToken)
  {
    MoveModel? move = await _pipeline.ExecuteAsync(new UpdateMoveCommand(id, payload), cancellationToken);
    return GetActionResult(move);
  }

  private ActionResult<MoveModel> GetActionResult(CreateOrReplaceMoveResult result) => GetActionResult(result.Move, result.Created);
  private ActionResult<MoveModel> GetActionResult(MoveModel? move, bool created = false)
  {
    if (move == null)
    {
      return NotFound();
    }
    if (created)
    {
      Uri location = HttpContext.BuildLocation("moves/{id}", [new KeyValuePair<string, string>("id", move.Id.ToString())]);
      return Created(location, move);
    }

    return Ok(move);
  }
}
