using Logitar.Portal.Contracts.Sessions;
using MediatR;

namespace PokeGame.Application.Accounts.Events;

public record UserSignedInEvent(Session Session) : INotification;
