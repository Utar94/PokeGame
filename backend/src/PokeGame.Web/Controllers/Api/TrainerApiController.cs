using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Application.Trainers.Mutations;
using PokeGame.Application.Trainers.Queries;
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
    private readonly IMediator _mediator;

    public TrainerApiController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<TrainerModel>> CreateAsync([FromBody] CreateTrainerPayload payload, CancellationToken cancellationToken)
    {
      TrainerModel trainer = await _mediator.Send(new CreateTrainerMutation(payload), cancellationToken);
      var uri = new Uri($"/api/trainers/{trainer.Id}", UriKind.Relative);

      return Created(uri, trainer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TrainerModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      await _mediator.Send(new DeleteTrainerMutation(id), cancellationToken);

      return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<TrainerModel>> GetAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new GetTrainersQuery
      {
        Gender = gender,
        Region = region,
        Search = search,
        UserId = userId,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainerModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      TrainerModel? trainer = await _mediator.Send(new GetTrainerQuery(id), cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }

      return Ok(trainer);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TrainerModel>> UpdateAsync(Guid id, [FromBody] UpdateTrainerPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _mediator.Send(new UpdateTrainerMutation(id, payload), cancellationToken));
    }
  }
}
