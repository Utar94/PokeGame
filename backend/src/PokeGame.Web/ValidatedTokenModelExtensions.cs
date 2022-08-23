using Logitar.Portal.Core.Tokens.Models;
using System.Text;
using System.Text.Json;

namespace PokeGame.Web
{
  internal static class ValidatedTokenModelExtensions
  {
    public static void EnsureHasSucceeded(this ValidatedTokenModel validatedToken)
    {
      ArgumentNullException.ThrowIfNull(validatedToken);

      if (!validatedToken.Succeeded)
      {
        var message = new StringBuilder();

        message.AppendLine("The token validation failed.");
        foreach (var error in validatedToken.Errors)
        {
          message.AppendLine(error.Description == null ? error.Code : $"{error.Code}: {error.Description}");
        }

        var json = JsonSerializer.Serialize(validatedToken.Errors);
        message.AppendLine(json);

        throw new InvalidOperationException(message.ToString());
      }
    }
  }
}
