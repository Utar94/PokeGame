using Logitar;
using Logitar.Identity.Domain.Shared;
using Logitar.Portal.Contracts.Errors;
using PokeGame.Contracts.Errors;

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
  public string? PropertyName
  {
    get => (string?)Data[nameof(PropertyName)];
    private set => Data[nameof(PropertyName)] = value;
  }

  public override Error Error => new PropertyError(this.GetErrorCode(), ErrorMessage, PropertyName, UniqueName);

  public UniqueNameAlreadyUsedException(Type type, UniqueNameUnit uniqueName, string? propertyName = null)
    : base(BuildMessage(type, uniqueName, propertyName))
  {
    TypeName = type.GetNamespaceQualifiedName();
    UniqueName = uniqueName.Value;
    PropertyName = propertyName;
  }

  private static string BuildMessage(Type type, UniqueNameUnit uniqueName, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(UniqueName), uniqueName.Value)
    .AddData(nameof(PropertyName), propertyName, "<null>")
    .Build();
}

public class UniqueNameAlreadyUsedException<T> : UniqueNameAlreadyUsedException
{
  public UniqueNameAlreadyUsedException(UniqueNameUnit uniqueName, string? propertyName = null) : base(typeof(T), uniqueName, propertyName)
  {
  }
}
