using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Inventories;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Inventories.Queries;
using PokeGame.Application.Models;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Application.Pokedex.Queries;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Pokemon.Queries;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Application.Trainers.Queries;
using PokeGame.Domain;
using PokeGame.Infrastructure;
using PokeGame.Web.Models.Api.Game;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
  [Route("api/game")]
  public class GameApiController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public GameApiController(IMediator mediator, IUserContext userContext)
    {
      _mediator = mediator;
      _userContext = userContext;
    }

    [HttpGet("trainers")]
    public async Task<ActionResult<IEnumerable<GameTrainerModel>>> GetTrainersAsync(CancellationToken cancellationToken)
    {
      ListModel<TrainerModel> trainers = await _mediator.Send(new GetTrainersQuery
      {
        UserId = _userContext.Id,
        Sort = TrainerSort.Name,
        Desc = false,
      }, cancellationToken);

      return Ok(trainers.Items.Select(trainer => new GameTrainerModel(trainer)));
    }

    [HttpGet("trainers/{id}/inventory")]
    public async Task<ActionResult<GameInventoryModel>> GetTrainerInventoryAsync(Guid id, CancellationToken cancellationToken)
    {
      TrainerModel? trainer = await _mediator.Send(new GetTrainerQuery(id), cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }
      else if (trainer.User?.Id != _userContext.Id)
      {
        return Forbid();
      }

      ListModel<InventoryModel> inventory = await _mediator.Send(new GetInventoryQuery
      {
        TrainerId = id,
        Sort = InventorySort.ItemName,
        Desc = false
      }, cancellationToken);

      return Ok(new GameInventoryModel(inventory));
    }

    [HttpGet("trainers/{id}/pokedex")]
    public async Task<ActionResult<GamePokedexModel>> GetTrainerPokedexAsync(Guid id, bool national, CancellationToken cancellationToken)
    {
      TrainerModel? trainer = await _mediator.Send(new GetTrainerQuery(id), cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }
      else if (trainer.User?.Id != _userContext.Id || national && !trainer.NationalPokedex)
      {
        return Forbid();
      }

      Region? region = national ? null : trainer.Region;

      ListModel<PokedexModel> pokedex = await _mediator.Send(new GetPokedexEntriesQuery
      {
        Region = region,
        TrainerId = trainer.Id
      }, cancellationToken);

      return Ok(new GamePokedexModel(pokedex.Items, trainer.NationalPokedex, region));
    }

    [HttpGet("trainers/{id}/pokemon")]
    public async Task<ActionResult<IEnumerable<GamePokemonModel>>> GetTrainerPokemonAsync(Guid id, CancellationToken cancellationToken)
    {
      TrainerModel? trainer = await _mediator.Send(new GetTrainerQuery(id), cancellationToken);
      if (trainer == null)
      {
        return NotFound();
      }
      else if (trainer.User?.Id != _userContext.Id)
      {
        return Forbid();
      }

      ListModel<PokemonModel> pokemonList = await _mediator.Send(new GetPokemonListQuery
      {
        TrainerId = trainer.Id,
        Sort = PokemonSort.Position,
        Desc = false
      }, cancellationToken);

      return Ok(pokemonList.Items.Where(x => x.Position.HasValue && x.History?.Trainer != null && x.Species != null)
        .Select(x => new GamePokemonModel(x)));
    }

    [HttpGet("trainers/pokemon/{id}")]
    public async Task<ActionResult<PokemonSummary>> GetPokemonSummaryAsync(Guid id, CancellationToken cancellationToken)
    {
      PokemonModel? pokemon = await _mediator.Send(new GetPokemonQuery(id), cancellationToken);
      if (pokemon == null)
      {
        return NotFound();
      }
      else if (pokemon.History?.Trainer?.User?.Id != _userContext.Id)
      {
        return Forbid();
      }

      return Ok(new PokemonSummary(pokemon));
    }
  }
}
