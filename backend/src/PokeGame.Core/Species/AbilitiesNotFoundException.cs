using System.Net;
using System.Text;

namespace PokeGame.Core.Species
{
  internal class AbilitiesNotFoundException : ApiException
  {
    public AbilitiesNotFoundException(IEnumerable<Guid> ids)
      : base(HttpStatusCode.NotFound, GetMessage(ids))
    {
      Ids = ids ?? throw new ArgumentNullException(nameof(ids));
    }

    public IEnumerable<Guid> Ids { get; }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified abilities could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");

      return message.ToString();
    }
  }
}
