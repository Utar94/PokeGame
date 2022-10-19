namespace PokeGame.Web.Models.Users
{
  public class ResetPasswordModel
  {
    public ResetPasswordModel(string token)
    {
      Token = token;
    }

    public string Token { get; }
  }
}
