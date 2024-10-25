﻿using Logitar.Portal.Contracts.Passwords;
using Logitar.Portal.Contracts.Users;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts;

public interface IOneTimePasswordService
{
  Task<OneTimePassword> CreateAsync(User user, string purpose, CancellationToken cancellationToken = default);
  Task<OneTimePassword> CreateAsync(User user, Phone? phone, string purpose, CancellationToken cancellationToken = default);
  Task<OneTimePassword> ValidateAsync(OneTimePasswordPayload payload, string purpose, CancellationToken cancellationToken = default);
}
