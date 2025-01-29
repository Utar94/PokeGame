using Logitar;
using Logitar.EventSourcing;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Errors;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Regions;

namespace PokeGame.Application;

public class UniqueNameAlreadyUsedException : ConflictException
{
  private const string ErrorMessage = "The specified unique name is already used.";

  public Guid EntityId
  {
    get => (Guid)Data[nameof(EntityId)]!;
    private set => Data[nameof(EntityId)] = value;
  }
  public Guid ConflictId
  {
    get => (Guid)Data[nameof(ConflictId)]!;
    private set => Data[nameof(ConflictId)] = value;
  }
  public string UniqueName
  {
    get => (string)Data[nameof(UniqueName)]!;
    private set => Data[nameof(UniqueName)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public override Error Error => new(this.GetErrorCode(), ErrorMessage, new Dictionary<string, object?>
  {
    [nameof(EntityId)] = EntityId,
    [nameof(ConflictId)] = ConflictId,
    [nameof(UniqueName)] = UniqueName,
    [nameof(PropertyName)] = PropertyName
  });

  public UniqueNameAlreadyUsedException(Ability ability, AbilityId conflictId)
    : this(ability.Id.StreamId, conflictId.StreamId, ability.UniqueName)
  {
  }
  public UniqueNameAlreadyUsedException(Move move, MoveId conflictId)
    : this(move.Id.StreamId, conflictId.StreamId, move.UniqueName)
  {
  }
  public UniqueNameAlreadyUsedException(Region region, RegionId conflictId)
    : this(region.Id.StreamId, conflictId.StreamId, region.UniqueName)
  {
  }

  private UniqueNameAlreadyUsedException(StreamId entityId, StreamId conflictId, UniqueName uniqueName, string propertyName = "UniqueName")
    : base(BuildMessage(entityId, conflictId, uniqueName, propertyName))
  {
    EntityId = entityId.ToGuid();
    ConflictId = conflictId.ToGuid();
    UniqueName = uniqueName.Value;
    PropertyName = propertyName;
  }

  private static string BuildMessage(StreamId entityId, StreamId conflictId, UniqueName uniqueName, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(EntityId), entityId)
    .AddData(nameof(ConflictId), conflictId)
    .AddData(nameof(UniqueName), uniqueName)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
