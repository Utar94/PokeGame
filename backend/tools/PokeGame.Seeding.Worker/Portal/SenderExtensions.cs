using Logitar.Portal.Contracts.Senders;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Seeding.Worker.Portal;

internal static class SenderExtensions
{
  public static string? GetKey(this CreateSenderPayload sender)
  {
    if (!string.IsNullOrEmpty(sender.EmailAddress))
    {
      return $"{ContactType.Email}:{sender.EmailAddress}";
    }
    else if (!string.IsNullOrEmpty(sender.PhoneNumber))
    {
      return $"{ContactType.Phone}:{sender.PhoneNumber}";
    }

    return null;
  }
  public static string? GetKey(this Sender sender)
  {
    if (!string.IsNullOrEmpty(sender.EmailAddress))
    {
      return $"{ContactType.Email}:{sender.EmailAddress}";
    }
    else if (!string.IsNullOrEmpty(sender.PhoneNumber))
    {
      return $"{ContactType.Phone}:{sender.PhoneNumber}";
    }

    return null;
  }
}
