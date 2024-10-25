using Logitar.Portal.Contracts.Sessions;

namespace PokeGame.Contracts.Accounts;

public record ResetPasswordResult
{
  public SentMessage? RecoveryLinkSentTo { get; set; }
  public Session? Session { get; set; }

  public ResetPasswordResult()
  {
  }

  public static ResetPasswordResult RecoveryLinkSent(SentMessage to) => new()
  {
    RecoveryLinkSentTo = to
  };

  public static ResetPasswordResult Success(Session session) => new()
  {
    Session = session
  };
}
