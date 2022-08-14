namespace PokeGame.Web.Models.Api.Account
{
  public class SignInModel
  {
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool Remember { get; set; }
  }
}
