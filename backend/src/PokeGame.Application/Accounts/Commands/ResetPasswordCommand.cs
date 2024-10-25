﻿using FluentValidation;
using Logitar;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Messages;
using Logitar.Portal.Contracts.Realms;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Tokens;
using Logitar.Portal.Contracts.Users;
using MediatR;
using PokeGame.Application.Accounts.Constants;
using PokeGame.Application.Accounts.Events;
using PokeGame.Application.Accounts.Validators;
using PokeGame.Contracts.Accounts;
using Locale = PokeGame.Domain.Locale;

namespace PokeGame.Application.Accounts.Commands;

public record ResetPasswordCommand(ResetPasswordPayload Payload, IEnumerable<CustomAttribute> CustomAttributes) : Activity, IRequest<ResetPasswordResult>
{
  public override IActivity Anonymize()
  {
    ResetPasswordCommand command = this.DeepClone();
    if (Payload.Reset != null && command.Payload.Reset != null)
    {
      command.Payload.Reset = new(Payload.Reset.Token, Payload.Reset.Password.Mask());
    }
    return command;
  }
}

internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResult>
{
  private readonly IMessageService _messageService;
  private readonly IPublisher _publisher;
  private readonly IRealmService _realmService;
  private readonly ISessionService _sessionService;
  private readonly ITokenService _tokenService;
  private readonly IUserService _userService;

  public ResetPasswordCommandHandler(
    IMessageService messageService,
    IPublisher publisher,
    IRealmService realmService,
    ISessionService sessionService,
    ITokenService tokenService,
    IUserService userService)
  {
    _messageService = messageService;
    _publisher = publisher;
    _realmService = realmService;
    _sessionService = sessionService;
    _tokenService = tokenService;
    _userService = userService;
  }

  public async Task<ResetPasswordResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
  {
    Realm realm = await _realmService.FindAsync(cancellationToken);

    ResetPasswordPayload payload = command.Payload;
    new ResetPasswordValidator(realm.PasswordSettings).ValidateAndThrow(payload);
    Locale locale = new(payload.Locale);

    if (!string.IsNullOrWhiteSpace(payload.EmailAddress))
    {
      return await HandleEmailAddressAsync(payload.EmailAddress.Trim(), locale, cancellationToken);
    }
    else if (payload.Reset != null)
    {
      return await HandleResetAsync(payload.Reset, command.CustomAttributes, cancellationToken);
    }

    throw new ArgumentException($"Exactly one of the following must be specified: {nameof(payload.EmailAddress)}, {nameof(payload.Reset)}.", nameof(command));
  }

  private async Task<ResetPasswordResult> HandleEmailAddressAsync(string emailAddress, Locale locale, CancellationToken cancellationToken)
  {
    SentMessages sentMessages;

    User? user = await _userService.FindAsync(emailAddress, cancellationToken);
    if (user == null || !user.HasPassword)
    {
      sentMessages = new([Guid.NewGuid()]);
    }
    else
    {
      CreatedToken passwordRecovery = await _tokenService.CreateAsync(user, TokenTypes.PasswordRecovery, cancellationToken);
      Dictionary<string, string> variables = new()
      {
        [Variables.Token] = passwordRecovery.Token
      };
      sentMessages = await _messageService.SendAsync(Templates.PasswordRecovery, user, ContactType.Email, locale, variables, cancellationToken);
    }

    Email email = new(emailAddress);
    SentMessage sentMessage = sentMessages.ToSentMessage(email);
    return ResetPasswordResult.RecoveryLinkSent(sentMessage);
  }

  private async Task<ResetPasswordResult> HandleResetAsync(ResetPayload payload, IEnumerable<CustomAttribute> customAttributes, CancellationToken cancellationToken)
  {
    ValidatedToken validatedToken = await _tokenService.ValidateAsync(payload.Token, TokenTypes.PasswordRecovery, cancellationToken);
    Guid userId = validatedToken.GetUserId();
    User user = await _userService.FindAsync(userId, cancellationToken) ?? throw new ArgumentException($"The user 'Id={userId}' could not be found.", nameof(payload));
    user = await _userService.ResetPasswordAsync(user, payload.Password, cancellationToken);

    Session session = await _sessionService.CreateAsync(user, customAttributes, cancellationToken);
    await _publisher.Publish(new UserSignedInEvent(session), cancellationToken);
    return ResetPasswordResult.Success(session);
  }
}
