using PokeGame.Domain;
using System.Text;

namespace PokeGame.Application
{
  public class EntityNotFoundException : Exception
  {
    public EntityNotFoundException(Type type, string id, string? paramName = null)
      : base(GetMessage(type, id, paramName))
    {
      Data["Type"] = type.GetName();
      Data["Id"] = id;

      if (paramName != null)
      {
        Data["ParamName"] = paramName;
      }
    }

    private static string GetMessage(Type type, string id, string? paramName)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified entity could not be found.");
      message.AppendLine($"Type: {type.GetName()}");
      message.AppendLine($"Id: {id}");

      if (paramName != null)
      {
        message.AppendLine($"ParamName: {paramName}");
      }

      return message.ToString();
    }
  }

  public class EntityNotFoundException<T> : EntityNotFoundException where T : Aggregate
  {
    public EntityNotFoundException(Guid id, string? paramName = null)
      : this(id.ToString(), paramName)
    {
    }
    public EntityNotFoundException(string id, string? paramName = null)
      : base(typeof(T), id, paramName)
    {
    }
  }
}
