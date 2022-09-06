using Logitar.Portal.Core.Users.Models;

namespace PokeGame.Web.Models.Users
{
  public class ProfileModel
  {
    public ProfileModel(UserModel user)
    {
      ArgumentNullException.ThrowIfNull(user);

      CreatedAt = user.CreatedAt;
      PasswordChangedAt = user.PasswordChangedAt;
      SignedInAt = user.SignedInAt;
      UpdatedAt = user.UpdatedAt;

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

    public DateTime CreatedAt { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    public DateTime? SignedInAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

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
