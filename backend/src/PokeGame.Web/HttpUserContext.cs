﻿using PokeGame.Infrastructure;
using System.Net;

namespace PokeGame.Web
{
  internal class HttpUserContext : IUserContext
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    protected HttpContext HttpContext => _httpContextAccessor.HttpContext
      ?? throw new InvalidOperationException($"The {nameof(_httpContextAccessor.HttpContext)} is required.");

    public Guid Id => HttpContext.GetUser()?.Id
      ?? throw new InvalidOperationException("An authenticated user is required.");

    public Guid SessionId => HttpContext.GetSession()?.Id
      ?? throw new InvalidOperationException("A session is required.");
  }
}
