using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/trainers")]
  public class TrainerApiController : ControllerBase
  {
    private readonly ITrainerService _service;

    public TrainerApiController(ITrainerService service)
    {
      _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<TrainerModel>> CreateAsync([FromBody] CreateTrainerPayload payload, CancellationToken cancellationToken)
    {
      TrainerModel trainer = await _service.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/api/trainers/{trainer.Id}", UriKind.Relative);

      return Created(uri, trainer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TrainerModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _service.DeleteAsync(id, cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<TrainerModel>> GetAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _service.GetAsync(gender, region, search, userId,
        sort, desc,
        index, count,
        cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainerModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      TrainerModel? trainer = await _service.GetAsync(id, cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }

      return Ok(trainer);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TrainerModel>> UpdateAsync(Guid id, [FromBody] UpdateTrainerPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
