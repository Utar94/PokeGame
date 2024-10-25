﻿using Logitar;
using Logitar.EventSourcing;

namespace PokeGame.EntityFrameworkCore.Entities;

internal abstract class AggregateEntity
{
  public string AggregateId { get; private set; } = string.Empty;
  public long Version { get; private set; }

  public string CreatedBy { get; private set; } = string.Empty;
  public DateTime CreatedOn { get; private set; }

  public string UpdatedBy { get; private set; } = string.Empty;
  public DateTime UpdatedOn { get; private set; }

  protected AggregateEntity()
  {
  }

  protected AggregateEntity(DomainEvent @event)
  {
    AggregateId = @event.AggregateId.Value;

    CreatedBy = @event.ActorId.Value;
    CreatedOn = @event.OccurredOn.AsUniversalTime();

    Update(@event);
  }

  public IEnumerable<ActorId> GetActorIds() => [new(CreatedBy), new(UpdatedBy)];

  protected virtual void Update(DomainEvent @event)
  {
    Version = @event.Version;

    UpdatedBy = @event.ActorId.Value;
    UpdatedOn = @event.OccurredOn.AsUniversalTime();
  }
}
