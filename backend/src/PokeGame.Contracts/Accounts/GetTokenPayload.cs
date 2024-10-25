namespace PokeGame.Contracts.Accounts;

public record GetTokenPayload : SignInPayload
{
  [JsonPropertyName("refresh_token")]
  public string? RefreshToken { get; set; }
}
