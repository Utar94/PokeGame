﻿using Logitar.Net.Http;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.ApiKeys;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using Microsoft.Extensions.Primitives;
using PokeGame.Application.Logging;
using PokeGame.Constants;
using PokeGame.Settings;
using System.Text.Json;

namespace PokeGame.Extensions;

internal static class HttpContextExtensions
{
  private const string ApiKeyKey = nameof(ApiKey);
  private const string SessionKey = nameof(Session);
  private const string SessionIdKey = "SessionId";
  private const string UserKey = nameof(User);
  private const string WorldKey = "World";

  public static Uri GetBaseUri(this HttpContext context)
  {
    string host = context.Request.Host.Value;
    int index = host.IndexOf(':');

    IUrlBuilder builder = new UrlBuilder().SetScheme(context.Request.Scheme, inferPort: true);
    if (index < 0)
    {
      builder.SetHost(host);
    }
    else
    {
      builder.SetHost(host[..index]).SetPort(ushort.Parse(host[(index + 1)..]));
    }
    return builder.BuildUri();
  }
  public static Uri BuildLocation(this HttpContext context, string path, IEnumerable<KeyValuePair<string, string>> parameters)
  {
    UrlBuilder builder = new(context.GetBaseUri());
    builder.SetPath(path);
    foreach (KeyValuePair<string, string> parameter in parameters)
    {
      builder.SetParameter(parameter.Key, parameter.Value);
    }
    return builder.BuildUri();
  }

  public static IEnumerable<CustomAttribute> GetSessionCustomAttributes(this HttpContext context)
  {
    List<CustomAttribute> customAttributes = new(capacity: 2)
    {
      new("AdditionalInformation", context.GetAdditionalInformation())
    };

    string? ipAddress = context.GetClientIpAddress();
    if (ipAddress != null)
    {
      customAttributes.Add(new("IpAddress", ipAddress));
    }

    return customAttributes;
  }
  public static string GetAdditionalInformation(this HttpContext context)
  {
    return JsonSerializer.Serialize(context.Request.Headers);
  }
  public static string? GetClientIpAddress(this HttpContext context)
  {
    string? ipAddress = null;

    if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues xForwardedFor))
    {
      ipAddress = xForwardedFor.Single()?.Split(':').First();
    }
    ipAddress ??= context.Connection.RemoteIpAddress?.ToString();

    return ipAddress;
  }

  public static ApiKey? GetApiKey(this HttpContext context) => context.GetItem<ApiKey>(ApiKeyKey);
  public static Session? GetSession(this HttpContext context) => context.GetItem<Session>(SessionKey);
  public static User? GetUser(this HttpContext context) => context.GetItem<User>(UserKey);
  private static T? GetItem<T>(this HttpContext context, object key) => context.Items.TryGetValue(key, out object? value) ? (T?)value : default;

  public static void SetApiKey(this HttpContext context, ApiKey? apiKey)
  {
    context.SetItem(ApiKeyKey, apiKey);
    context.GetLoggingService().SetApiKey(apiKey);
  }
  public static void SetSession(this HttpContext context, Session? session)
  {
    context.SetItem(SessionKey, session);
    context.GetLoggingService().SetSession(session);
  }
  public static void SetUser(this HttpContext context, User? user)
  {
    context.SetItem(UserKey, user);
    context.GetLoggingService().SetUser(user);
  }
  private static void SetItem(this HttpContext context, object key, object? value)
  {
    if (value == null)
    {
      context.Items.Remove(key);
    }
    else
    {
      context.Items[key] = value;
    }
  }

  private static ILoggingService GetLoggingService(this HttpContext context) => context.RequestServices.GetRequiredService<ILoggingService>();

  public static Guid? GetSessionId(this HttpContext context)
  {
    byte[]? bytes = context.Session.Get(SessionIdKey);
    return bytes == null ? null : new Guid(bytes);
  }
  public static bool IsSignedIn(this HttpContext context) => context.GetSessionId().HasValue;
  public static void SignIn(this HttpContext context, Session session)
  {
    context.Session.Set(SessionIdKey, session.Id.ToByteArray());

    if (session.RefreshToken != null)
    {
      CookiesSettings cookiesSettings = context.RequestServices.GetRequiredService<CookiesSettings>();
      CookieOptions options = new()
      {
        HttpOnly = cookiesSettings.RefreshToken.HttpOnly,
        MaxAge = cookiesSettings.RefreshToken.MaxAge,
        SameSite = cookiesSettings.RefreshToken.SameSite,
        Secure = cookiesSettings.RefreshToken.Secure
      };
      context.Response.Cookies.Append(Cookies.RefreshToken, session.RefreshToken, options);
    }

    context.SetSession(session);
    context.SetUser(session.User);
  }
  public static void SignOut(this HttpContext context)
  {
    context.Session.Clear();

    context.Response.Cookies.Delete(Cookies.RefreshToken);
  }
}
