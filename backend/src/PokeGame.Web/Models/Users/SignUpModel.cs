namespace PokeGame.Web.Models.Users
{
  public class SignUpModel
  {
    public SignUpModel(string email, string token)
    {
      Email = email;
      Token = token;
    }

    public string Email { get; }
    public string Token { get; }
  }
}
