using Logitar;
using Logitar.Portal.Contracts;
using MediatR;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Commands;

public record SignInCommand(SignInPayload Payload, IEnumerable<CustomAttribute> CustomAttributes) : Activity, IRequest<SignInCommandResult>
{
  public override IActivity Anonymize()
  {
    SignInCommand command = this.DeepClone();
    if (command.Payload.Credentials != null && Payload.Credentials?.Password != null)
    {
      command.Payload.Credentials.Password = Payload.Credentials.Password.Mask();
    }
    return command;
  }
}
