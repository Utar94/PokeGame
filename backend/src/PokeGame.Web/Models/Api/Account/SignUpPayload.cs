namespace PokeGame.Web.Models.Api.Account
{
  public class SignUpPayload
  {
    public string Token { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Locale { get; set; } = null!;
  }
}
