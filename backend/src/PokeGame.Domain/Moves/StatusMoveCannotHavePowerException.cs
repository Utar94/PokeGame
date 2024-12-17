using Logitar;
using PokeGame.Domain.Errors;

namespace PokeGame.Domain.Moves;

public class StatusMoveCannotHavePowerException : ErrorException
{
  private const string ErrorMessage = "A move in the Status category cannot have Power.";

  public Guid MoveId
  {
    get => (Guid)Data[nameof(MoveId)]!;
    private set => Data[nameof(MoveId)] = value;
  }

  public override Error Error => new(this.GetErrorCode(), ErrorMessage);

  public StatusMoveCannotHavePowerException(Move move) : base(BuildMessage(move))
  {
    MoveId = move.Id.ToGuid();
  }

  private static string BuildMessage(Move move) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(MoveId), move.Id.ToGuid())
    .Build();
}
