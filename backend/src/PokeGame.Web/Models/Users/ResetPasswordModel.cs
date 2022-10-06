namespace PokeGame.Web.Models.Users
{
  public class ResetPasswordModel
  {
    public ResetPasswordModel(string token)
    {
      Token = token ?? throw new ArgumentNullException(nameof(token));
    }

    public string Token { get; }
  }
}
