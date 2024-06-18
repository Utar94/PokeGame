using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using PokeGame.Contracts.Abilities;
using PokeGame.Contracts.Moves;
using PokeGame.Contracts.Regions;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

internal class Mapper
{
  private readonly Dictionary<ActorId, Actor> _actors;

  public Mapper()
  {
    _actors = [];
  }

  public Mapper(IEnumerable<Actor> actors) : this()
  {
    foreach (Actor actor in actors)
    {
      ActorId id = new(actor.Id);
      _actors[id] = actor;
    }
  }

  public static Actor ToActor(ActorEntity source) => new(source.DisplayName)
  {
    Id = new ActorId(source.Id).ToGuid(),
    Type = source.Type,
    IsDeleted = source.IsDeleted,
    EmailAddress = source.EmailAddress,
    PictureUrl = source.PictureUrl
  };

  public Ability ToAbility(AbilityEntity source)
  {
    Ability destination = new(source.UniqueName)
    {
      DisplayName = source.DisplayName,
      Description = source.Description,
      Reference = source.Reference,
      Notes = source.Notes
    };

    MapAggregate(source, destination);

    return destination;
  }

  public Move ToMove(MoveEntity source)
  {
    Move destination = new(source.UniqueName)
    {
      Type = source.Type,
      Category = source.Category,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Accuracy = source.Accuracy,
      Power = source.Power,
      PowerPoints = source.PowerPoints,
      Reference = source.Reference,
      Notes = source.Notes
    };

    foreach (KeyValuePair<string, int> statisticChange in source.StatisticChanges)
    {
      destination.StatisticChanges.Add(new StatisticChange(statisticChange));
    }
    foreach (KeyValuePair<string, int> statusCondition in source.StatusConditions)
    {
      destination.StatusConditions.Add(new InflictedStatusCondition(statusCondition));
    }

    MapAggregate(source, destination);

    return destination;
  }

  public Region ToRegion(RegionEntity source)
  {
    Region destination = new(source.UniqueName)
    {
      DisplayName = source.DisplayName,
      Description = source.Description,
      Reference = source.Reference,
      Notes = source.Notes
    };

    MapAggregate(source, destination);

    return destination;
  }

  private void MapAggregate(AggregateEntity source, Aggregate destination)
  {
    destination.Id = new AggregateId(source.AggregateId).ToGuid();
    destination.Version = source.Version;

    destination.CreatedBy = FindActor(source.CreatedBy);
    destination.CreatedOn = AsUniversalTime(source.CreatedOn);

    destination.UpdatedBy = FindActor(source.UpdatedBy);
    destination.UpdatedOn = AsUniversalTime(source.UpdatedOn);
  }

  private Actor FindActor(string id) => FindActor(new ActorId(id));
  private Actor FindActor(ActorId id) => _actors.TryGetValue(id, out Actor? actor) ? actor : Actor.System;

  private static DateTime? AsUniversalTime(DateTime? value) => value.HasValue ? AsUniversalTime(value.Value) : null;
  private static DateTime AsUniversalTime(DateTime value) => value.Kind switch
  {
    DateTimeKind.Local => value.ToUniversalTime(),
    DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
    _ => value,
  };
}
