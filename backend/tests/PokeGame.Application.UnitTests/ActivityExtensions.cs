using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application;

internal static class ActivityExtensions
{
  public static void Contextualize(this Activity activity)
  {
    activity.Contextualize(user: null);
  }
  public static void Contextualize(this Activity activity, User? user)
  {
    ActivityContext context = new(Session: null, user);
    activity.Contextualize(context);
  }
}
