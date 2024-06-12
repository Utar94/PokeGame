using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Search;
using Logitar.Portal.Contracts.Senders;
using MediatR;

namespace PokeGame.Seeding.Worker.Portal.Commands;

internal class SeedSendersCommandHandler : INotificationHandler<SeedSendersCommand>
{
  private readonly IConfiguration _configuration;
  private readonly ILogger<SeedSendersCommandHandler> _logger;
  private readonly ISenderClient _senders;

  public SeedSendersCommandHandler(IConfiguration configuration, ILogger<SeedSendersCommandHandler> logger, ISenderClient senders)
  {
    _configuration = configuration;
    _logger = logger;
    _senders = senders;
  }

  public async Task Handle(SeedSendersCommand _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    SearchResults<Sender> results = await _senders.SearchAsync(new SearchSendersPayload(), context);
    Dictionary<string, Sender> senders = new(capacity: results.Items.Count);
    foreach (Sender sender in results.Items)
    {
      string? key = sender.GetKey();
      if (key != null)
      {
        senders[key] = sender;
      }
    }

    IEnumerable<CreateSenderPayload> payloads = _configuration.GetSection("Portal:Senders").GetChildren()
      .Select(section => section.Get<CreateSenderPayload>() ?? new());
    foreach (CreateSenderPayload payload in payloads)
    {
      string? key = payload.GetKey();
      if (key != null)
      {
        string status = "updated";
        string[] values = key.Split(':');
        if (senders.TryGetValue(key, out Sender? sender))
        {
          ReplaceSenderPayload replace = new()
          {
            EmailAddress = payload.EmailAddress,
            PhoneNumber = payload.PhoneNumber,
            DisplayName = payload.DisplayName,
            Description = payload.Description,
            Mailgun = payload.Mailgun,
            SendGrid = payload.SendGrid,
            Twilio = payload.Twilio
          };
          sender = await _senders.ReplaceAsync(sender.Id, replace, sender.Version, context)
            ?? throw new InvalidOperationException($"The sender 'Id={sender.Id}' replace result should not be null.");
        }
        else
        {
          sender = await _senders.CreateAsync(payload, context);
          senders[key] = sender;
          status = "created";
        }

        _logger.LogInformation("The {ContactType} sender '{ContactValue}' has been {Status} (Id={Id}).", values[0], values[1], status, sender.Id);
      }
    }
  }
}
