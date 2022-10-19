using Logitar.Portal.Core.Users.Models;

namespace PokeGame.ReadModel.Entities
{
  internal class UserEntity : Entity
  {
    public string Username { get; private set; } = string.Empty;

    public string? Email { get; private set; }
    public string? FullName { get; private set; }

    public string? Locale { get; private set; }
    public string? Picture { get; private set; }

    public List<TrainerEntity> Trainers { get; private set; } = new();

    public void Synchronize(UserModel user)
    {
      CreatedOn = user.CreatedAt;
      CreatedById = user.CreatedBy?.Id ?? Guid.Empty;

      UpdatedOn = user.UpdatedAt;
      UpdatedById = user.UpdatedBy?.Id;

      Version = user.Version;

      Username = user.Username;

      Email = user.Email;
      FullName = user.FullName;

      Locale = user.Locale;
      Picture = user.Picture;
    }
  }
}
