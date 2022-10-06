namespace PokeGame.Web.Models.Api.User
{
  public class InviteUserPayload
  {
    public string Email { get; set; } = string.Empty;
    public string? Locale { get; set; }
  }
}
