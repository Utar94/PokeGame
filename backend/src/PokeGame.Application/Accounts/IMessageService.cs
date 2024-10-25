using Logitar.Portal.Contracts.Messages;
using Logitar.Portal.Contracts.Users;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain;

namespace PokeGame.Application.Accounts;

public interface IMessageService
{
  Task<SentMessages> SendAsync(string template, Email email, Locale locale, IReadOnlyDictionary<string, string> variables, CancellationToken cancellationToken = default);
  Task<SentMessages> SendAsync(string template, Phone phone, Locale locale, IReadOnlyDictionary<string, string> variables, CancellationToken cancellationToken = default);
  Task<SentMessages> SendAsync(string template, User user, ContactType contactType, Locale locale, IReadOnlyDictionary<string, string> variables, CancellationToken cancellationToken = default);
}
