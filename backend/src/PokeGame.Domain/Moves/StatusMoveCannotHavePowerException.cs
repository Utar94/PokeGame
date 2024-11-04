using Logitar;
using Logitar.Portal.Contracts.Errors;
using PokeGame.Contracts.Errors;

namespace PokeGame.Domain.Moves;

public class StatusMoveCannotHavePowerException : DomainException
{
  private const string ErrorMessage = "A status move cannot have power.";

  public Guid MoveId
  {
    get => (Guid)Data[nameof(MoveId)]!;
    private set => Data[nameof(MoveId)] = value;
  }
  public int Power
  {
    get => (int)Data[nameof(Power)]!;
    private set => Data[nameof(Power)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public override Error Error => new PropertyError(this.GetErrorCode(), ErrorMessage, Power, PropertyName);

  public StatusMoveCannotHavePowerException(Move move, int power, string propertyName) : base(BuildMessage(move, power, propertyName))
  {
    MoveId = move.Id.ToGuid();
    Power = power;
    PropertyName = propertyName;
  }

  private static string BuildMessage(Move move, int power, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(MoveId), move.Id.ToGuid())
    .AddData(nameof(Power), power)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
