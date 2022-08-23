namespace PokeGame.Web.Models.Users
{
  public class SignUpModel
  {
    public SignUpModel(string email, string token)
    {
      Email = email ?? throw new ArgumentNullException(nameof(email));
      Token = token ?? throw new ArgumentNullException(nameof(token));
    }

    public string Email { get; }
    public string Token { get; }
  }
}
