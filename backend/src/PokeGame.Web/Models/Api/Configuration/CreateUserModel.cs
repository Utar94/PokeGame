namespace PokeGame.Web.Models.Api.Configuration
{
  public class CreateUserModel
  {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Locale { get; set; } = string.Empty;
  }
}
