using FluentValidation;
using Logitar.Portal.Contracts.Users;
using MediatR;
using PokeGame.Application.Accounts.Events;
using PokeGame.Application.Accounts.Validators;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Commands;

public record SaveProfileCommand(SaveProfilePayload Payload) : Activity, IRequest<User>;

internal class SaveProfileCommandHandler : IRequestHandler<SaveProfileCommand, User>
{
  private readonly IPublisher _publisher;
  private readonly IUserService _userService;

  public SaveProfileCommandHandler(IPublisher publisher, IUserService userService)
  {
    _publisher = publisher;
    _userService = userService;
  }

  public async Task<User> Handle(SaveProfileCommand command, CancellationToken cancellationToken)
  {
    SaveProfilePayload payload = command.Payload;
    new SaveProfileValidator().ValidateAndThrow(payload);

    User user = command.GetUser();
    user = await _userService.SaveProfileAsync(user, payload, cancellationToken);
    await _publisher.Publish(new UserUpdatedEvent(user), cancellationToken);

    return user;
  }
}
