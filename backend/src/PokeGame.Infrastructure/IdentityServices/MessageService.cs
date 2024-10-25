﻿using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Messages;
using Logitar.Portal.Contracts.Users;
using Microsoft.Extensions.Configuration;
using PokeGame.Application.Accounts;
using PokeGame.Contracts.Accounts;
using Locale = PokeGame.Domain.Locale;

namespace PokeGame.Infrastructure.IdentityServices;

internal class MessageService : IMessageService
{
  private readonly IMessageClient _messageClient;
  private readonly Guid _phoneSenderId;

  public MessageService(IConfiguration configuration, IMessageClient messageClient)
  {
    _messageClient = messageClient;
    _phoneSenderId = configuration.GetSection("Portal").GetValue<Guid?>("PhoneSenderId")
      ?? throw new ArgumentException("The configuration key 'Portal:PhoneSenderId' is required.", nameof(configuration));
  }

  public async Task<SentMessages> SendAsync(string template, Email email, Locale locale, IReadOnlyDictionary<string, string> variables, CancellationToken cancellationToken)
  {
    RecipientPayload recipient = new()
    {
      Address = email.Address
    };
    return await SendAsync(template, recipient, ContactType.Email, locale, variables, cancellationToken);
  }

  public async Task<SentMessages> SendAsync(string template, Phone phone, Locale locale, IReadOnlyDictionary<string, string> variables, CancellationToken cancellationToken)
  {
    RecipientPayload recipient = new()
    {
      PhoneNumber = phone.E164Formatted
    };
    return await SendAsync(template, recipient, ContactType.Phone, locale, variables, cancellationToken);
  }

  public async Task<SentMessages> SendAsync(string template, User user, ContactType contactType, Locale locale, IReadOnlyDictionary<string, string> variables, CancellationToken cancellationToken)
  {
    RecipientPayload recipient = new()
    {
      UserId = user.Id
    };
    return await SendAsync(template, recipient, contactType, locale, variables, cancellationToken);
  }

  private async Task<SentMessages> SendAsync(string template, RecipientPayload recipient, ContactType contactType, Locale locale, IEnumerable<KeyValuePair<string, string>>? variables, CancellationToken cancellationToken)
  {
    SendMessagePayload payload = new(template)
    {
      Locale = locale.Code
    };
    if (contactType == ContactType.Phone)
    {
      payload.SenderId = _phoneSenderId;
    }
    payload.Recipients.Add(recipient);
    if (variables != null)
    {
      foreach (KeyValuePair<string, string> variable in variables)
      {
        payload.Variables.Add(new Variable(variable));
      }
    }
    RequestContext context = new(recipient.UserId?.ToString(), cancellationToken);
    return await _messageClient.SendAsync(payload, context);
  }
}
