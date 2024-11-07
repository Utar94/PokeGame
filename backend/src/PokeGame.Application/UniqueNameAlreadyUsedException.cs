using Logitar;
using Logitar.Portal.Contracts.Errors;
using PokeGame.Contracts.Errors;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Regions;

namespace PokeGame.Application;

public class UniqueNameAlreadyUsedException : ConflictException
{
  private const string ErrorMessage = "The specified unique name is already used.";

  public string TypeName
  {
    get => (string)Data[nameof(TypeName)]!;
    private set => Data[nameof(TypeName)] = value;
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
  public IReadOnlyCollection<Guid> ConflictingIds
  {
    get => (IReadOnlyCollection<Guid>)Data[nameof(ConflictingIds)]!;
    private set => Data[nameof(ConflictingIds)] = value;
  }

  public override Error Error
  {
    get
    {
      PropertyError error = new(this.GetErrorCode(), ErrorMessage, UniqueName, PropertyName);
      error.AddData(nameof(ConflictingIds), string.Join(',', ConflictingIds));
      return error;
    }
  }

  public UniqueNameAlreadyUsedException(Ability ability, AbilityId conflictId)
    : this(ability.GetType(), ability.UniqueName, nameof(ability.UniqueName), [ability.Id.ToGuid(), conflictId.ToGuid()])
  {
  }
  public UniqueNameAlreadyUsedException(Move move, MoveId conflictId)
    : this(move.GetType(), move.UniqueName, nameof(move.UniqueName), [move.Id.ToGuid(), conflictId.ToGuid()])
  {
  }
  public UniqueNameAlreadyUsedException(Region region, RegionId conflictId)
    : this(region.GetType(), region.UniqueName, nameof(region.UniqueName), [region.Id.ToGuid(), conflictId.ToGuid()])
  {
  }
  private UniqueNameAlreadyUsedException(Type type, UniqueName uniqueName, string propertyName, IEnumerable<Guid> conflictingIds)
    : base(BuildMessage(type, uniqueName, propertyName, conflictingIds))
  {
    TypeName = type.GetNamespaceQualifiedName();
    UniqueName = uniqueName.Value;
    PropertyName = propertyName;
    ConflictingIds = conflictingIds.ToArray().AsReadOnly();
  }

  private static string BuildMessage(Type type, UniqueName uniqueName, string propertyName, IEnumerable<Guid> conflictingIds)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append(nameof(TypeName)).Append(": ").AppendLine(type.GetNamespaceQualifiedName());
    message.Append(nameof(UniqueName)).Append(": ").Append(uniqueName).AppendLine();
    message.Append(nameof(PropertyName)).Append(": ").AppendLine(propertyName);

    message.Append(nameof(ConflictingIds)).Append(':').AppendLine();
    foreach (Guid conflictingId in conflictingIds)
    {
      message.Append(" - ").Append(conflictingId).AppendLine();
    }

    return message.ToString();
  }
}
