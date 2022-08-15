using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core;
using PokeGame.Core.Models;
using PokeGame.Core.Moves;
using PokeGame.Core.Moves.Models;
using PokeGame.Core.Moves.Payloads;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/moves")]
  public class MoveApiController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IMoveService _service;

    public MoveApiController(IMapper mapper, IMoveService service)
    {
      _mapper = mapper;
      _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<MoveModel>> CreateAsync([FromBody] CreateMovePayload payload, CancellationToken cancellationToken)
    {
      MoveModel move = await _service.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/api/moves/{move.Id}", UriKind.Relative);

      return Created(uri, move);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MoveModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _service.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<MoveSummary>> GetAsync(string? search, PokemonType? type,
      MoveSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      ListModel<MoveModel> moves = await _service.GetAsync(search, type,
        sort, desc,
        index, count,
        cancellationToken);

      return Ok(new ListModel<MoveSummary>(_mapper.Map<IEnumerable<MoveSummary>>(moves.Items), moves.Total));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MoveModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      MoveModel? move = await _service.GetAsync(id, cancellationToken);
      if (move == null)
      {
        return NotFound();
      }

      return Ok(move);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MoveModel>> UpdateAsync(Guid id, [FromBody] UpdateMovePayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
