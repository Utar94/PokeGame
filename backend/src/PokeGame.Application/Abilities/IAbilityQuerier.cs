﻿using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

public interface IAbilityQuerier
{
  Task<AbilityModel> ReadAsync(Ability ability, CancellationToken cancellationToken = default);
  Task<AbilityModel?> ReadAsync(AbilityId id, CancellationToken cancellationToken = default);
  Task<AbilityModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);

  Task<SearchResults<AbilityModel>> SearchAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken = default);
}