using FluentValidation;
using PokeGame.Core.Abilities;
using PokeGame.Core.Models;
using PokeGame.Core.Species.Models;
using PokeGame.Core.Species.Payloads;

namespace PokeGame.Core.Species
{
  internal class SpeciesService : ISpeciesService
  {
    private readonly IAbilityQuerier _abilityQuerier;
    private readonly IMappingService _mappingService;
    private readonly ISpeciesQuerier _querier;
    private readonly IRepository<Species> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Species> _validator;

    public SpeciesService(
      IAbilityQuerier abilityQuerier,
      IMappingService mappingService,
      ISpeciesQuerier querier,
      IRepository<Species> repository,
      IUserContext userContext,
      IValidator<Species> validator
    )
    {
      _abilityQuerier = abilityQuerier;
      _mappingService = mappingService;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<SpeciesModel> CreateAsync(CreateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      Ability? ability = await ResolveAbilityAsync(payload.AbilityId, cancellationToken);

      var species = new Species(payload, _userContext.Id, ability);
      _validator.ValidateAndThrow(species);

      await _repository.SaveAsync(species, cancellationToken);

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    public async Task<SpeciesModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Species species = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Species>(id);

      species.Delete(_userContext.Id);

      await _repository.SaveAsync(species, cancellationToken);

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    public async Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Species? species = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (species == null)
      {
        return null;
      }

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    public async Task<ListModel<SpeciesModel>> GetAsync(string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Species> species = await _querier.GetPagedAsync(search, type,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Species, SpeciesModel>(species, cancellationToken);
    }

    public async Task<SpeciesModel> UpdateAsync(Guid id, UpdateSpeciesPayload payload, CancellationToken cancellationToken)
    {
      Ability? ability = await ResolveAbilityAsync(payload.AbilityId, cancellationToken);

      Species species = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Species>(id);

      species.Update(payload, _userContext.Id, ability);
      _validator.ValidateAndThrow(species);

      await _repository.SaveAsync(species, cancellationToken);

      return await _mappingService.MapAsync<SpeciesModel>(species, cancellationToken);
    }

    private async Task<Ability?> ResolveAbilityAsync(Guid? id, CancellationToken cancellationToken)
    {
      Ability? ability = null;

      if (id.HasValue)
      {
        ability = await _abilityQuerier.GetAsync(id.Value, readOnly: false, cancellationToken)
          ?? throw new EntityNotFoundException<Ability>(id.Value, nameof(SaveSpeciesPayload.AbilityId));
      }

      return ability;
    }
  }
}
