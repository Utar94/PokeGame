using FluentValidation;
using PokeGame.Domain.Moves;

namespace PokeGame.Application
{
  internal static class ValidationContextExtensions
  {
    private const string MoveCategoryKey = nameof(MoveCategory);

    public static MoveCategory GetMoveCategory<T>(this ValidationContext<T> context)
    {
      ArgumentNullException.ThrowIfNull(context);

      return context.RootContextData.TryGetValue(MoveCategoryKey, out object? value)
        ? (MoveCategory)value
        : default;
    }

    public static void SetMoveCategory<T>(this ValidationContext<T> context, MoveCategory category)
    {
      ArgumentNullException.ThrowIfNull(context);

      context.RootContextData[MoveCategoryKey] = category;
    }
  }
}
