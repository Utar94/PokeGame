namespace PokeGame.Web.Models.Api.Account
{
  public class SaveProfileModel
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }

    public string? Locale { get; set; }
    public string? Picture { get; set; }
  }
}
