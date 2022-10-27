using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class UsePokemonMoveMutationHandler : IRequestHandler<UsePokemonMoveMutation, IEnumerable<PokemonModel>>
  {
    private static readonly Random _random = new();

    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<UsePokemonMovePayload> _usePokemonMoveValidator;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public UsePokemonMoveMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<UsePokemonMovePayload> usePokemonMoveValidator,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _usePokemonMoveValidator = usePokemonMoveValidator;
      _validator = validator;
    }

    public async Task<IEnumerable<PokemonModel>> Handle(UsePokemonMoveMutation request, CancellationToken cancellationToken)
    {
      UsePokemonMovePayload payload = request.Payload;

      Move move = await _repository.LoadAsync<Move>(request.MoveId, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(request.MoveId);

      var context = ValidationContext<UsePokemonMovePayload>.CreateWithOptions(payload, options => options.ThrowOnFailures());
      context.SetMoveCategory(move.Category);
      _usePokemonMoveValidator.Validate(context);

      HashSet<Guid> pokemonIds = new[] { request.Id }.Concat(payload.Targets!.Select(x => x.Id)).ToHashSet();
      Dictionary<Guid, Domain.Pokemon.Pokemon> pokemonIndex = (await _repository.LoadAsync<Domain.Pokemon.Pokemon>(pokemonIds, cancellationToken))
        .ToDictionary(x => x.Id, x => x);

      IEnumerable<Guid> missingIds = pokemonIds.Except(pokemonIndex.Keys).Distinct();
      if (missingIds.Any())
      {
        throw new PokemonNotFoundException(missingIds);
      }

      Domain.Pokemon.Pokemon attacker = pokemonIndex[request.Id];
      attacker.UseMove(move, payload);
      _validator.ValidateAndThrow(attacker);

      Ability ability = await _repository.LoadAsync<Ability>(attacker.AbilityId, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(attacker.AbilityId);
      Domain.Species.Species species = await _repository.LoadAsync<Domain.Species.Species>(attacker.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(attacker.SpeciesId);

      bool multipleTargets = payload.Targets!.Count() > 1;
      foreach (TargetPayload target in payload.Targets!)
      {
        Domain.Pokemon.Pokemon targetPokemon = pokemonIndex[target.Id];

        ushort damage = 0;
        if (move.Category != MoveCategory.Status)
        {
          damage = CalculateDamage(move, attacker, ability, species, targetPokemon, multipleTargets, payload.Damage!, target);
        }

        targetPokemon.Wound(damage, attacker.Level, payload.StatusCondition);
        _validator.ValidateAndThrow(targetPokemon);
      }

      await _repository.SaveAsync(pokemonIndex.Values, cancellationToken);

      return await _querier.GetAsync(pokemonIds, cancellationToken);
    }

    private static ushort CalculateDamage(
      Move move,
      Domain.Pokemon.Pokemon attacker,
      Ability ability,
      Domain.Species.Species species,
      Domain.Pokemon.Pokemon target,
      bool multipleTargets,
      DamagePayload payload,
      TargetPayload targetPayload
    )
    {
      if (targetPayload.Effectiveness == 0)
      {
        return 0;
      }

      ushort attack = 0;
      if (payload.Attack.HasValue!)
      {
        attack = payload.Attack.Value;
      }
      else if (move.Category == MoveCategory.Physical)
      {
        attack = attacker.Attack;
      }
      else if (move.Category == MoveCategory.Special)
      {
        attack = attacker.SpecialAttack;
      }

      ushort defense = 0;
      if (targetPayload.Defense.HasValue)
      {
        defense = targetPayload.Defense.Value;
      }
      else if (move.Category == MoveCategory.Physical)
      {
        defense = target.Defense;
      }
      else if (move.Category == MoveCategory.Special)
      {
        defense = target.SpecialDefense;
      }

      double damage = (((2 * attacker.Level / 5.0) + 2) * (payload.Power ?? move.Power ?? 0) * (attack / (double)defense) / 50.0) + 2;

      if (multipleTargets)
      {
        damage *= 0.75;
      }

      switch (payload.Weather)
      {
        case Weather.HarshSunlight:
          if (move.Type == PokemonType.Fire)
          {
            damage *= 1.5;
          }
          else if (move.Type == PokemonType.Water)
          {
            damage *= 0.5;
          }
          break;
        case Weather.Rain:
          if (move.Type == PokemonType.Water)
          {
            damage *= 1.5;
          }
          else if (move.Type == PokemonType.Fire)
          {
            damage *= 0.5;
          }
          break;
      }

      if (payload.IsCritical)
      {
        damage *= 1.5;
      }

      damage *= payload.Random ?? _random.Next(85, 100 + 1) / 100.0;

      if (payload.STAB.HasValue)
      {
        damage *= payload.STAB.Value;
      }
      else if (move.Type == species.PrimaryType || move.Type == species.SecondaryType)
      {
        double stab = ability.Name == "Adaptability" ? 2.0 : 1.5;
        damage *= stab;
      }

      if (targetPayload.Effectiveness.HasValue)
      {
        damage *= targetPayload.Effectiveness.Value;
      }

      if (payload.IsBurnt == true || (attacker.StatusCondition == StatusCondition.Burn && ability.Name != "Guts" && move.Category == MoveCategory.Physical && move.Name != "Facade"))
      {
        damage *= 0.5;
      }

      if (targetPayload.OtherModifiers.HasValue)
      {
        damage *= targetPayload.OtherModifiers.Value;
      }

      return (ushort)Math.Max(1, Math.Floor(damage));
    }
  }
}
