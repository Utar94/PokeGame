namespace PokeGame.Application.Accounts;

public interface IGoogleService
{
  Task<GoogleIdentity> GetIdentityAsync(string token, CancellationToken cancellationToken = default);
}
