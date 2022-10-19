using System.Text;

namespace PokeGame.Domain
{
  internal class EventNotSupportedException : NotSupportedException
  {
    public EventNotSupportedException(Type aggregateType, Type eventType)
      : base(GetMessage(aggregateType, eventType))
    {
      Data["AggregateType"] = aggregateType.GetName();
      Data["EventType"] = eventType.GetName();
    }

    private static string GetMessage(Type aggregateType, Type eventType)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified event is not supported by the aggregate.");
      message.AppendLine($"Aggregate type: {aggregateType.GetName()}");
      message.AppendLine($"Event type: {eventType.GetName()}");

      return message.ToString();
    }
  }
}
