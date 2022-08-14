namespace PokeGame.Web.Models.Api.Configuration
{
  public class CreateUserModel
  {
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Locale { get; set; } = null!;
  }
}
