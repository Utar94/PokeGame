﻿using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Users;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts;

public interface IUserService
{
  Task<User> AuthenticateAsync(string uniqueName, string password, CancellationToken cancellationToken = default);
  Task<User> AuthenticateAsync(User user, string password, CancellationToken cancellationToken = default);
  Task<User> ChangePasswordAsync(User user, ChangeAccountPasswordPayload payload, CancellationToken cancellationToken = default);
  Task<User> CompleteProfileAsync(User user, CompleteProfilePayload payload, PhonePayload? phone, CancellationToken cancellationToken = default);
  Task<User> CreateAsync(EmailPayload email, CancellationToken cancellationToken = default);
  Task<User> CreateAsync(EmailPayload email, CustomIdentifier? identifier, CancellationToken cancellationToken = default);
  Task<User?> FindAsync(string emailAddress, CancellationToken cancellationToken = default);
  Task<User?> FindAsync(Guid id, CancellationToken cancellationToken = default);
  Task<User?> FindAsync(CustomIdentifier identifier, CancellationToken cancellationToken = default);
  Task<User> ResetPasswordAsync(User user, string password, CancellationToken cancellationToken = default);
  Task SignOutAsync(Guid id, CancellationToken cancellationToken = default);
  Task<User> SaveIdentifierAsync(User user, CustomIdentifier identifier, CancellationToken cancellationToken = default);
  Task<User> SaveProfileAsync(User user, SaveProfilePayload payload, CancellationToken cancellationToken = default);
  Task<User> UpdateAsync(User user, EmailPayload email, CancellationToken cancellationToken = default);
  Task<User> UpdateAsync(User user, PhonePayload phone, CancellationToken cancellationToken = default);
}
