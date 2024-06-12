namespace PokeGame.Seeding.Worker.Portal;

internal record UserSummary(string EmailAddress, string Password, string FirstName, string LastName, string Locale, string TimeZone, string? Picture, bool IsGamemaster);
