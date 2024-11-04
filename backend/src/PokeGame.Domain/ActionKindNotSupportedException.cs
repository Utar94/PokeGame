using Logitar;
using PokeGame.Contracts;

namespace PokeGame.Domain;

public class ActionKindNotSupportedException : NotSupportedException
{
  private const string ErrorMessage = "The specified action kind is not supported.";

  public ActionKind ActionKind
  {
    get => (ActionKind)Data[nameof(ActionKind)]!;
    private set => Data[nameof(ActionKind)] = value;
  }

  public ActionKindNotSupportedException(ActionKind actionKind) : base(BuildMessage(actionKind))
  {
    ActionKind = actionKind;
  }

  private static string BuildMessage(ActionKind actionKind) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ActionKind), actionKind)
    .Build();
}
