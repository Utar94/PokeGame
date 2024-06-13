using Logitar.EventSourcing;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class LogEventEntity
{
  public long LogEventId { get; private set; }

  public LogEntity? Log { get; private set; }
  public long LogId { get; private set; }
  public string EventId { get; private set; } = string.Empty;

  public LogEventEntity(LogEntity log, DomainEvent @event)
  {
    Log = log;
    LogId = log.LogId;
    EventId = @event.Id.Value;
  }

  private LogEventEntity()
  {
  }
}
