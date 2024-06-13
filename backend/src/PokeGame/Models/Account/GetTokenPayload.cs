using PokeGame.Contracts.Accounts;

namespace PokeGame.Models.Account;

public record GetTokenPayload : SignInPayload
{
  [JsonPropertyName("refresh_token")]
  public string? RefreshToken { get; set; }
}
