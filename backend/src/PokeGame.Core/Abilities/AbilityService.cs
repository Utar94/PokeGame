using FluentValidation;
using PokeGame.Core.Abilities.Models;
using PokeGame.Core.Abilities.Payloads;
using PokeGame.Core.Models;

namespace PokeGame.Core.Abilities
{
  internal class AbilityService : IAbilityService
  {
    private readonly IMappingService _mappingService;
    private readonly IAbilityQuerier _querier;
    private readonly IRepository<Ability> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Ability> _validator;

    public AbilityService(
      IMappingService mappingService,
      IAbilityQuerier querier,
      IRepository<Ability> repository,
      IUserContext userContext,
      IValidator<Ability> validator
    )
    {
      _mappingService = mappingService;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<AbilityModel> CreateAsync(CreateAbilityPayload payload, CancellationToken cancellationToken)
    {
      var ability = new Ability(payload, _userContext.Id);
      _validator.ValidateAndThrow(ability);

      await _repository.SaveAsync(ability, cancellationToken);

      return await _mappingService.MapAsync<AbilityModel>(ability, cancellationToken);
    }

    public async Task<AbilityModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Ability ability = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(id);

      ability.Delete(_userContext.Id);

      await _repository.SaveAsync(ability, cancellationToken);

      return await _mappingService.MapAsync<AbilityModel>(ability, cancellationToken);
    }

    public async Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Ability? ability = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (ability == null)
      {
        return null;
      }

      return await _mappingService.MapAsync<AbilityModel>(ability, cancellationToken);
    }

    public async Task<ListModel<AbilityModel>> GetAsync(string? search,
      AbilitySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Ability> abilities = await _querier.GetPagedAsync(search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Ability, AbilityModel>(abilities, cancellationToken);
    }

    public async Task<AbilityModel> UpdateAsync(Guid id, UpdateAbilityPayload payload, CancellationToken cancellationToken)
    {
      Ability ability = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(id);

      ability.Update(payload, _userContext.Id);
      _validator.ValidateAndThrow(ability);

      await _repository.SaveAsync(ability, cancellationToken);

      return await _mappingService.MapAsync<AbilityModel>(ability, cancellationToken);
    }
  }
}
