using Logitar.Portal.Contracts;
using MediatR;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Commands;

public record SignInCommand(SignInPayload Payload, IEnumerable<CustomAttribute> CustomAttributes) : IRequest<SignInCommandResult>;
