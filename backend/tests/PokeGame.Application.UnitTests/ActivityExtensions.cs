namespace PokeGame.Application;

internal static class ActivityExtensions
{
  public static void Contextualize(this Activity activity)
  {
    ActivityContext context = new(Session: null, User: null);
    activity.Contextualize(context);
  }
}
