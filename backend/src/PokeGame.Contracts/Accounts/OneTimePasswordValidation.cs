using Logitar.Portal.Contracts.Passwords;

namespace PokeGame.Contracts.Accounts;

public record OneTimePasswordValidation
{
  public Guid OneTimePasswordId { get; set; }
  public SentMessage SentMessage { get; set; }

  public OneTimePasswordValidation(OneTimePassword oneTimePassword, SentMessage sentMessage) : this(oneTimePassword.Id, sentMessage)
  {
  }

  public OneTimePasswordValidation(Guid oneTimePasswordId, SentMessage sentMessage)
  {
    OneTimePasswordId = oneTimePasswordId;
    SentMessage = sentMessage;
  }
}
