using PokeGame.Application.Models;

namespace PokeGame.Web.Models.Users
{
  public class UserSummary
  {
    public Guid Id { get; set; }

    public ActorModel? UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }

    public string? FullName { get; set; }

    public string? Picture { get; set; }

    public DateTime? PasswordChangedOn { get; set; }
    public DateTime? SignedInOn { get; set; }
  }
}
