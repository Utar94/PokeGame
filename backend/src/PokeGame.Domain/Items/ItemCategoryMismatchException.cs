using Logitar;
using PokeGame.Contracts.Items;

namespace PokeGame.Domain.Items;

public class ItemCategoryMismatchException : Exception
{
  public const string ErrorMessage = "The specified item category was not expected.";

  public string ItemId
  {
    get => (string)Data[nameof(ItemId)]!;
    private set => Data[nameof(ItemId)] = value;
  }
  public ItemCategory ExpectedType
  {
    get => (ItemCategory)Data[nameof(ExpectedType)]!;
    private set => Data[nameof(ExpectedType)] = value;
  }
  public ItemCategory ActualType
  {
    get => (ItemCategory)Data[nameof(ActualType)]!;
    private set => Data[nameof(ActualType)] = value;
  }

  public ItemCategoryMismatchException(ItemAggregate fieldType, ItemCategory actualType) : base(BuildMessage(fieldType, actualType))
  {
    ItemId = fieldType.Id.Value;
    ExpectedType = fieldType.Category;
    ActualType = actualType;
  }

  private static string BuildMessage(ItemAggregate fieldType, ItemCategory actualType) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ItemId), fieldType.Id.Value)
    .AddData(nameof(ExpectedType), fieldType.Category)
    .AddData(nameof(ActualType), actualType)
    .Build();
}
