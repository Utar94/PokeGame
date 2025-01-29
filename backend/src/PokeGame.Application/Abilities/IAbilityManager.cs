using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities;

public interface IAbilityManager
{
  Task SaveAsync(Ability ability, CancellationToken cancellationToken = default);
}
