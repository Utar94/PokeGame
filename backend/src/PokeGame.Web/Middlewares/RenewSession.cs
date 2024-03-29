﻿using Logitar.Portal.Client;
using Logitar.Portal.Core.Accounts.Payloads;
using Logitar.Portal.Core.Sessions.Models;
using PokeGame.Web.Settings;
using System.Text.Json;

namespace PokeGame.Web.Middlewares
{
  internal class RenewSession
  {
    private readonly RequestDelegate _next;

    public RenewSession(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAccountService accountService, ClientPortalSettings portalSettings)
    {
      if (!context.HasSession())
      {
        if (context.Request.Cookies.TryGetValue(Constants.Cookies.RenewToken, out string? renewToken) && renewToken != null)
        {
          try
          {
            var payload = new RenewSessionPayload
            {
              AdditionalInformation = JsonSerializer.Serialize(context.Request.Headers),
              IpAddress = context.Connection.RemoteIpAddress?.ToString(),
              RenewToken = renewToken,
            };
            SessionModel session = await accountService.RenewSessionAsync(portalSettings.Realm, payload);
            context.SignIn(session);
          }
          catch (Exception)
          {
            context.Response.Cookies.Delete(Constants.Cookies.RenewToken);
          }
        }
      }

      await _next(context);
    }
  }
}
