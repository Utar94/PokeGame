using Logitar.Identity.Contracts.Settings;

namespace PokeGame.Domain;

internal record ReadOnlyUniqueNameSettings : IUniqueNameSettings
{
  public string? AllowedCharacters { get; }

  public ReadOnlyUniqueNameSettings() : this("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+")
  {
  }

  public ReadOnlyUniqueNameSettings(IUniqueNameSettings uniqueName) : this(uniqueName.AllowedCharacters)
  {
  }

  public ReadOnlyUniqueNameSettings(string? allowedCharacters)
  {
    AllowedCharacters = allowedCharacters;
  }
}
