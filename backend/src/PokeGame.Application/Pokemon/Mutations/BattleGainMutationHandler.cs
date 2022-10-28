using FluentValidation;
using MediatR;
using PokeGame.Application.Items;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Pokemon.Payloads;
using PokeGame.Domain.Items;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class BattleGainMutationHandler : IRequestHandler<BattleGainMutation, IEnumerable<PokemonModel>>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public BattleGainMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<IEnumerable<PokemonModel>> Handle(BattleGainMutation request, CancellationToken cancellationToken)
    {
      BattleGainPayload payload = request.Payload;

      if (payload.Winners?.Any() != true)
      {
        return Enumerable.Empty<PokemonModel>();
      }

      IEnumerable<Guid> pokemonIds = new[] { payload.DefeatedId }.Concat(payload.Winners.Select(x => x.Id)).Distinct();
      Dictionary<Guid, Domain.Pokemon.Pokemon> pokemonIndex = (await _repository.LoadAsync<Domain.Pokemon.Pokemon>(pokemonIds, cancellationToken))
        .ToDictionary(x => x.Id, x => x);
      IEnumerable<Guid> missingIds = pokemonIds.Except(pokemonIndex.Keys).Distinct();
      if (missingIds.Any())
      {
        throw new PokemonNotFoundException(missingIds);
      }

      IEnumerable<Guid> itemIds = pokemonIndex.Values.Where(x => x.HeldItemId.HasValue).Select(x => x.HeldItemId!.Value)
        .Concat(pokemonIndex.Values.Where(x => x.History != null).Select(x => x.History!.BallId))
        .Distinct();
      Dictionary<Guid, Item> items = itemIds.Any()
        ? (await _repository.LoadAsync<Item>(itemIds, cancellationToken)).ToDictionary(x => x.Id, x => x)
        : new();
      IEnumerable<Guid> missingItems = itemIds.Except(items.Keys).Distinct();
      if (missingItems.Any())
      {
        throw new ItemsNotFoundException(missingItems);
      }

      Domain.Pokemon.Pokemon defeatedPokemon = pokemonIndex[payload.DefeatedId];

      Domain.Species.Species defeatedSpecies = await _repository.LoadAsync<Domain.Species.Species>(defeatedPokemon.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(defeatedPokemon.SpeciesId);

      foreach (WinnerPokemonPayload winnerPayload in payload.Winners)
      {
        Domain.Pokemon.Pokemon winnerPokemon = pokemonIndex[winnerPayload.Id];

        Item? ball = winnerPokemon.History != null ? items[winnerPokemon.History.BallId] : null;
        Item? heldItem = winnerPokemon.HeldItemId.HasValue ? items[winnerPokemon.HeldItemId.Value] : null;

        var gainPayload = new ExperienceGainPayload
        {
          EffortValues = defeatedSpecies.EvYield.Select(pair => StatisticValuePayload.Create(pair)),
          Experience = CalculateExperienceGain(defeatedPokemon, defeatedSpecies, winnerPayload, winnerPokemon, heldItem, payload.IsTrainerBattle)
        };

        bool hasBeenCaughtWithLuxuryBall = ball?.Kind == ItemKind.LuxuryBall;
        bool isHoldingSootheBell = heldItem?.Kind == ItemKind.SootheBell;
        winnerPokemon.GainedExperience(gainPayload, hasBeenCaughtWithLuxuryBall, isHoldingSootheBell);
        _validator.ValidateAndThrow(winnerPokemon);
      }

      await _repository.SaveAsync(pokemonIndex.Values, cancellationToken);

      return await _querier.GetAsync(pokemonIds, cancellationToken);
    }

    private static uint CalculateExperienceGain(
      Domain.Pokemon.Pokemon defeatedPokemon,
      Domain.Species.Species defeatedSpecies,
      WinnerPokemonPayload winnerPayload,
      Domain.Pokemon.Pokemon winnerPokemon,
      Item? heldItem,
      bool isTrainerBattle
    )
    {
      double experience = (defeatedSpecies.BaseExperienceYield ?? 0) * defeatedPokemon.Level / 5.0;

      if (!winnerPayload.HasParticipated)
      {
        experience /= 2.0;
      }

      experience *= Math.Pow((2 * defeatedPokemon.Level + 10) / (double)(defeatedPokemon.Level + winnerPokemon.Level + 10), 2.5);
      experience += 1;

      if (isTrainerBattle)
      {
        experience *= 1.5;
      }
      if (winnerPokemon.IsTraded)
      {
        experience *= 1.5;
      }

      if (heldItem?.Kind == ItemKind.LuckyEgg)
      {
        experience *= 1.5;
      }

      if (winnerPayload.CanEvolve && winnerPokemon.HasHighFriendshid)
      {
        experience *= 1.5;
      }
      else if (winnerPayload.CanEvolve || winnerPokemon.HasHighFriendshid)
      {
        experience *= Math.Sqrt(1.5);
      }

      if (winnerPayload.OtherModifiers.HasValue)
      {
        experience *= winnerPayload.OtherModifiers.Value;
      }

      return (uint)Math.Floor(experience);
    }
  }
}
