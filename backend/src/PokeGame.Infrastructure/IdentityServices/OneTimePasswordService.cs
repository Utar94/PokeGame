﻿using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Passwords;
using Logitar.Portal.Contracts.Users;
using PokeGame.Application.Accounts;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Infrastructure.IdentityServices;

internal class OneTimePasswordService : IOneTimePasswordService
{
  private const string Characters = "0123456789";
  private const int Length = 6;
  private const int LifetimeSeconds = 60 * 60;
  private const int MaximumAttempts = 5;

  private readonly IOneTimePasswordClient _oneTimePasswordClient;

  public OneTimePasswordService(IOneTimePasswordClient oneTimePasswordClient)
  {
    _oneTimePasswordClient = oneTimePasswordClient;
  }

  public async Task<OneTimePassword> CreateAsync(User user, string purpose, CancellationToken cancellationToken)
  {
    return await CreateAsync(user, phone: null, purpose, cancellationToken);
  }
  public async Task<OneTimePassword> CreateAsync(User user, Phone? phone, string purpose, CancellationToken cancellationToken)
  {
    CreateOneTimePasswordPayload payload = new(Characters, Length)
    {
      ExpiresOn = DateTime.Now.AddSeconds(LifetimeSeconds),
      MaximumAttempts = MaximumAttempts
    };
    payload.SetUserId(user);
    if (phone != null)
    {
      payload.SetPhone(phone);
    }
    payload.SetPurpose(purpose);
    RequestContext context = new(user.Id.ToString(), cancellationToken);
    return await _oneTimePasswordClient.CreateAsync(payload, context);
  }

  public async Task<OneTimePassword> ValidateAsync(OneTimePasswordPayload oneTimePasswordPayload, string purpose, CancellationToken cancellationToken)
  {
    ValidateOneTimePasswordPayload payload = new(oneTimePasswordPayload.Code);
    RequestContext context = new(cancellationToken);
    OneTimePassword oneTimePassword = await _oneTimePasswordClient.ValidateAsync(oneTimePasswordPayload.Id, payload, context)
      ?? throw new OneTimePasswordNotFoundException(oneTimePasswordPayload.Id);
    oneTimePassword.EnsurePurpose(purpose);
    return oneTimePassword;
  }
}
