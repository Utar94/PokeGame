﻿using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application.Accounts;

public interface ISessionService
{
  Task<Session> CreateAsync(User user, IEnumerable<CustomAttribute> customAttributes, CancellationToken cancellationToken = default);
  Task<Session?> FindAsync(Guid id, CancellationToken cancellationToken = default);
  Task<Session> RenewAsync(string refreshToken, IEnumerable<CustomAttribute> customAttributes, CancellationToken cancellationToken = default);
  Task<Session> SignInAsync(User user, string password, IEnumerable<CustomAttribute> customAttributes, CancellationToken cancellationToken = default);
  Task SignOutAsync(Guid id, CancellationToken cancellationToken = default);
}
