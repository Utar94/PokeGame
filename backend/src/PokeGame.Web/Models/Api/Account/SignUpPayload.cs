namespace PokeGame.Web.Models.Api.Account
{
  public class SignUpPayload
  {
    public string Token { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Locale { get; set; } = string.Empty;
  }
}
