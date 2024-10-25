﻿using Logitar.Portal.Contracts.Tokens;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application.Accounts;

public interface ITokenService
{
  Task<CreatedToken> CreateAsync(User user, string type, CancellationToken cancellationToken = default);
  Task<CreatedToken> CreateAsync(User? user, Email email, string type, CancellationToken cancellationToken = default);
  Task<CreatedToken> CreateAsync(User? user, Phone phone, string type, CancellationToken cancellationToken = default);
  Task<ValidatedToken> ValidateAsync(string token, string type, CancellationToken cancellationToken = default);
  Task<ValidatedToken> ValidateAsync(string token, bool consume, string type, CancellationToken cancellationToken = default);
}
