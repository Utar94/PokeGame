using Logitar;
using Logitar.Portal.Contracts.Errors;
using PokeGame.Contracts.Errors;
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

  public UniqueNameAlreadyUsedException(Region region, RegionId conflictId) : base(BuildMessage(region, conflictId))
  {
    TypeName = region.GetType().GetNamespaceQualifiedName();
    UniqueName = region.UniqueName.Value;
    PropertyName = nameof(region.UniqueName);
    ConflictingIds = [region.Id.ToGuid(), conflictId.ToGuid()];
  }

  private static string BuildMessage(Region region, RegionId conflictId)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append(nameof(TypeName)).Append(": ").AppendLine(region.GetType().GetNamespaceQualifiedName());
    message.Append(nameof(UniqueName)).Append(": ").Append(region.UniqueName).AppendLine();
    message.Append(nameof(PropertyName)).Append(": ").Append(nameof(region.UniqueName)).AppendLine();

    message.Append(nameof(ConflictingIds)).Append(':').AppendLine();
    message.Append(" - ").Append(region.Id.ToGuid()).AppendLine();
    message.Append(" - ").Append(conflictId.ToGuid()).AppendLine();

    return message.ToString();
  }
}
