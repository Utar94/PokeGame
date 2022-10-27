namespace PokeGame.Web
{
  internal static class Constants
  {
    internal static class Cookies
    {
      public const string RenewToken = "renew_token";

      public static readonly CookieOptions RenewTokenOptions = new()
      {
        HttpOnly = true,
        MaxAge = TimeSpan.FromDays(7),
        SameSite = SameSiteMode.Strict,
        Secure = true
      };
    }

    internal static class CreateUser
    {
      public const int Lifetime = 7 * 24 * 60 * 60; // One week
      public const string Purpose = "create_user";
      public const string Template = "UserInvitation";
    }

    internal static class Policies
    {
      public const string Administrator = nameof(Administrator);
      public const string AuthenticatedUser = nameof(AuthenticatedUser);
    }

    internal static class Schemes
    {
      public const string Session = nameof(Session);

      public static string[] All => new[] { Session };
    }
  }
}
