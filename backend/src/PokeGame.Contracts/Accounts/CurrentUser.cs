﻿using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Contracts.Accounts;

public record CurrentUser
{
  public string DisplayName { get; set; }
  public string? EmailAddress { get; set; }
  public string? PictureUrl { get; set; }

  public CurrentUser() : this(string.Empty)
  {
  }

  public CurrentUser(string displayName, string? emailAddress = null, string? pictureUrl = null)
  {
    DisplayName = displayName;
    EmailAddress = emailAddress;
    PictureUrl = pictureUrl;
  }

  public CurrentUser(Session session) : this(session.User)
  {
  }

  public CurrentUser(User user) : this(user.FullName ?? user.UniqueName, user.Email?.Address, user.Picture)
  {
  }
}
