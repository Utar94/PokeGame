using Logitar.Portal.Core.Users.Models;

namespace PokeGame.Web.Models.Users
{
  public class ProfileModel
  {
    public ProfileModel(UserModel user)
    {
      CreatedOn = user.CreatedAt;
      PasswordChangedOn = user.PasswordChangedAt;
      SignedInOn = user.SignedInAt;
      UpdatedOn = user.UpdatedAt;

      Username = user.Username;

      Email = user.Email;
      IsEmailConfirmed = user.IsEmailConfirmed;
      PhoneNumber = user.PhoneNumber;
      IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed;

      FirstName = user.FirstName;
      MiddleName = user.MiddleName;
      LastName = user.LastName;
      FullName = user.FullName;

      Locale = user.Locale;
      Picture = user.Picture;
    }

    public DateTime CreatedOn { get; set; }
    public DateTime? PasswordChangedOn { get; set; }
    public DateTime? SignedInOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }

    public string? Locale { get; set; }
    public string? Picture { get; set; }
  }
}
