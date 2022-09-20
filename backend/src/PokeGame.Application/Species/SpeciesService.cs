using FluentValidation;
using PokeGame.Application.Items;
using PokeGame.Application.Models;
using PokeGame.Application.Moves;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species
{
  internal class SpeciesService : ISpeciesService
  {
    private readonly IRepository<Ability> _abilityRepository;
    private readonly IRepository<Item> _itemRepository;
    private readonly IRepository<Move> _moveRepository;
    private readonly ISpeciesQuerier _querier;
    private readonly IRepository<Domain.Species.Species> _repository;
    private readonly IValidator<Domain.Species.Species> _validator;

    public SpeciesService(
      IRepository<Ability> abilityRepository,
      IRepository<Item> itemRepository,
      IRepository<Move> moveRepository,
      ISpeciesQuerier querier,
      IRepository<Domain.Species.Species> repository,
      IValidator<Domain.Species.Species> validator
    )
    {
      _abilityRepository = abilityRepository;
      _itemRepository = itemRepository;
      _moveRepository = moveRepository;
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<SpeciesModel> CreateAsync(CreateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      await ValidateAbilitiesAsync(payload, cancellationToken);
      await ValidateEvolutionsAsync(payload, cancellationToken);

      var species = new Domain.Species.Species(payload);
      _validator.ValidateAndThrow(species);

      await _repository.SaveAsync(species, cancellationToken);

      return await _querier.GetAsync(species.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(species.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(id);

      species.Delete();

      await _repository.SaveAsync(species, cancellationToken);
    }

    public async Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(id, cancellationToken);
    }

    public async Task<ListModel<SpeciesModel>> GetAsync(string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(search, type,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<SpeciesModel> UpdateAsync(Guid id, UpdateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(id);

      await ValidateAbilitiesAsync(payload, cancellationToken);
      await ValidateEvolutionsAsync(payload, cancellationToken);

      species.Update(payload);
      _validator.ValidateAndThrow(species);

      await _repository.SaveAsync(species, cancellationToken);

      return await _querier.GetAsync(species.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(species.Id);
    }

    private async Task ValidateAbilitiesAsync(SaveSpeciesPayload payload, CancellationToken cancellationToken)
    {
      if (payload.AbilityIds?.Any() == true)
      {
        IEnumerable<Ability> abilities = await _abilityRepository.LoadAsync(payload.AbilityIds, cancellationToken);
        IEnumerable<Guid> missingIds = payload.AbilityIds.Except(abilities.Select(x => x.Id)).Distinct();
        if (missingIds.Any())
        {
          throw new AbilitiesNotFoundException(missingIds);
        }
      }
    }

    private async Task ValidateEvolutionsAsync(SaveSpeciesPayload payload, CancellationToken cancellationToken)
    {
      if (payload.Evolutions?.Any() == true)
      {
        IEnumerable<Guid> speciesIds = payload.Evolutions.Select(x => x.SpeciesId).Distinct();
        IEnumerable<Domain.Species.Species> species = await _repository.LoadAsync(speciesIds, cancellationToken);
        IEnumerable<Guid> missingSpeciesIds = speciesIds.Except(species.Select(x => x.Id)).Distinct();
        if (missingSpeciesIds.Any())
        {
          throw new SpeciesNotFoundException(missingSpeciesIds);
        }

        IEnumerable<Guid> itemIds = payload.Evolutions.Where(x => x.ItemId.HasValue).Select(x => x.ItemId!.Value).Distinct();
        if (itemIds.Any())
        {
          IEnumerable<Item> items = await _itemRepository.LoadAsync(itemIds, cancellationToken);
          IEnumerable<Guid> missingItemIds = itemIds.Except(items.Select(x => x.Id)).Distinct();
          if (missingItemIds.Any())
          {
            throw new ItemsNotFoundException(missingItemIds);
          }
        }

        IEnumerable<Guid> moveIds = payload.Evolutions.Where(x => x.MoveId.HasValue).Select(x => x.MoveId!.Value).Distinct();
        if (moveIds.Any())
        {
          IEnumerable<Move> moves = await _moveRepository.LoadAsync(moveIds, cancellationToken);
          IEnumerable<Guid> missingMoveIds = moveIds.Except(moves.Select(x => x.Id)).Distinct();
          if (missingMoveIds.Any())
          {
            throw new MovesNotFoundException(missingMoveIds);
          }
        }
      }
    }
  }
}
