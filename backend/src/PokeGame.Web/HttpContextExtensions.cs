using Logitar.Portal.Core.Sessions.Models;
using Logitar.Portal.Core.Users.Models;
using System.Text;
using System.Text.Json;

namespace PokeGame.Web
{
  internal static class HttpContextExtensions
  {
    private const string SessionKey = "Session";

    public static UserModel? GetUser(this HttpContext context) => context.GetSession()?.User;
    public static SessionModel? GetSession(this HttpContext context) => context.GetItem<SessionModel>(SessionKey);
    private static T? GetItem<T>(this HttpContext context, object key)
    {
      if (context.Items.TryGetValue(key, out object? value))
      {
        return (T?)value;
      }

      return default;
    }

    public static bool SetSession(this HttpContext context, SessionModel? session) => context.SetItem(SessionKey, session);
    private static bool SetItem<T>(this HttpContext context, object key, T? value)
    {
      if (context.Items.ContainsKey(key))
      {
        if (!context.Items.Remove(key))
        {
          return false;
        }
      }

      return value != null && context.Items.TryAdd(key, value);
    }

    public static bool HasSession(this HttpContext context) => context.ReadSession() != null;
    public static SessionModel? ReadSession(this HttpContext context)
    {
      if (context.Session.TryGetValue(SessionKey, out byte[]? bytes))
      {
        string json = Encoding.UTF8.GetString(bytes);

        return JsonSerializer.Deserialize<SessionModel>(json);
      }

      return null;
    }
    public static void SignIn(this HttpContext context, SessionModel session)
    {
      context.SetSession(SessionKey, session);

      if (session.RenewToken != null)
      {
        context.Response.Cookies.Append(Constants.Cookies.RenewToken, session.RenewToken, Constants.Cookies.RenewTokenOptions);
      }
    }

    private static void SetSession<T>(this HttpContext context, string key, T value)
    {
      string json = JsonSerializer.Serialize(value);
      byte[] bytes = Encoding.UTF8.GetBytes(json);

      context.Session.Set(key, bytes);
    }
  }
}
