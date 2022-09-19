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
  internal class UsePokemonMoveMutationHandler : IRequestHandler<UsePokemonMoveMutation, PokemonModel>
  {
    private static readonly Random _random = new();

    private readonly IRepository<Ability> _abilityRepository;
    private readonly IRepository<Move> _moveRepository;
    private readonly IPokemonQuerier _querier;
    private readonly IRepository<Domain.Pokemon.Pokemon> _repository;
    private readonly IRepository<Domain.Species.Species> _speciesRepository;
    private readonly IValidator<UsePokemonMovePayload> _usePokemonMoveValidator;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public UsePokemonMoveMutationHandler(
      IRepository<Ability> abilityRepository,
      IRepository<Move> moveRepository,
      IPokemonQuerier querier,
      IRepository<Domain.Pokemon.Pokemon> repository,
      IRepository<Domain.Species.Species> speciesRepository,
      IValidator<UsePokemonMovePayload> usePokemonMoveValidator,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _abilityRepository = abilityRepository;
      _moveRepository = moveRepository;
      _querier = querier;
      _repository = repository;
      _speciesRepository = speciesRepository;
      _usePokemonMoveValidator = usePokemonMoveValidator;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(UsePokemonMoveMutation request, CancellationToken cancellationToken)
    {
      UsePokemonMovePayload payload = request.Payload;

      Move move = await _moveRepository.LoadAsync(request.MoveId, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(request.MoveId);

      var context = ValidationContext<UsePokemonMovePayload>.CreateWithOptions(payload, options => options.ThrowOnFailures());
      context.SetMoveCategory(move.Category);
      _usePokemonMoveValidator.Validate(context);

      HashSet<Guid> pokemonIds = new[] { request.Id }.Concat(payload.Targets!.Select(x => x.Id)).ToHashSet();
      Dictionary<Guid, Domain.Pokemon.Pokemon> pokemonIndex = (await _repository.LoadAsync(pokemonIds, cancellationToken))
        .ToDictionary(x => x.Id, x => x);

      IEnumerable<Guid> missingIds = pokemonIds.Except(pokemonIndex.Keys).Distinct();
      if (missingIds.Any())
      {
        throw new PokemonNotFoundException(missingIds);
      }

      Domain.Pokemon.Pokemon pokemon = pokemonIndex[request.Id];
      pokemon.UseMove(move, payload);
      _validator.ValidateAndThrow(pokemon);

      Ability ability = await _abilityRepository.LoadAsync(pokemon.AbilityId, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(pokemon.AbilityId);
      Domain.Species.Species species = await _speciesRepository.LoadAsync(pokemon.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(pokemon.SpeciesId);

      bool multipleTargets = payload.Targets!.Count() > 1;
      foreach (TargetPayload target in payload.Targets!)
      {
        Domain.Pokemon.Pokemon targetPokemon = pokemonIndex[target.Id];

        short damage = 0;
        if (move.Category != MoveCategory.Status)
        {
          damage = CalculateDamage(move, pokemon, ability, species, targetPokemon, multipleTargets, payload.Damage!, target);
        }

        targetPokemon.Wound(damage, payload.StatusCondition);
        _validator.ValidateAndThrow(targetPokemon);
      }

      await _repository.SaveAsync(pokemonIndex.Values, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    private static short CalculateDamage(
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
      short attack = 0;
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

      short defense = 0;
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

      return (short)Math.Floor(damage);
    }
  }
}
