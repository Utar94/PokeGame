using Logitar.Portal.Contracts.Users;
using MediatR;

namespace PokeGame.Application.Accounts.Events;

public record UserUpdatedEvent(User User) : INotification;
