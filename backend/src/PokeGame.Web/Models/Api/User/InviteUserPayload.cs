namespace PokeGame.Web.Models.Api.User
{
  public class InviteUserPayload
  {
    public string Email { get; set; } = null!;
    public string? Locale { get; set; }
  }
}
