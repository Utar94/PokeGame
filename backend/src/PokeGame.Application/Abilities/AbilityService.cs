using FluentValidation;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Abilities.Payloads;

namespace PokeGame.Application.Abilities
{
  internal class AbilityService : IAbilityService
  {
    private readonly IAbilityQuerier _querier;
    private readonly IRepository<Ability> _repository;
    private readonly IValidator<Ability> _validator;

    public AbilityService(
      IAbilityQuerier querier,
      IRepository<Ability> repository,
      IValidator<Ability> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<AbilityModel> CreateAsync(CreateAbilityPayload payload, CancellationToken cancellationToken)
    {
      var ability = new Ability(payload);
      _validator.ValidateAndThrow(ability);

      await _repository.SaveAsync(ability, cancellationToken);

      return await _querier.GetAsync(ability.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(ability.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Ability ability = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(id);

      ability.Delete();

      await _repository.SaveAsync(ability, cancellationToken);
    }

    public async Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(id, cancellationToken);
    }

    public async Task<ListModel<AbilityModel>> GetAsync(string? search, Guid? speciesId,
      AbilitySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(search, speciesId,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<AbilityModel> UpdateAsync(Guid id, UpdateAbilityPayload payload, CancellationToken cancellationToken)
    {
      Ability ability = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(id);

      ability.Update(payload);
      _validator.ValidateAndThrow(ability);

      await _repository.SaveAsync(ability, cancellationToken);

      return await _querier.GetAsync(ability.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Ability>(ability.Id);
    }
  }
}
