using Logitar;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure;

internal class Mapper
{
  private readonly Dictionary<ActorId, ActorModel> _actors = [];
  private readonly ActorModel _system = ActorModel.System;

  public Mapper()
  {
  }

  public Mapper(IEnumerable<ActorModel> actors)
  {
    foreach (ActorModel actor in actors)
    {
      ActorId id = new(actor.Id);
      _actors[id] = actor;
    }
  }

  public AbilityModel ToAbility(AbilityEntity source)
  {
    AbilityModel destination = new()
    {
      UniqueName = source.UniqueName,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Link = source.Link,
      Notes = source.Notes
    };

    MapAggregate(source, destination);

    return destination;
  }

  public MoveModel ToMove(MoveEntity source)
  {
    MoveModel destination = new()
    {
      Type = source.Type,
      Category = source.Category,
      UniqueName = source.UniqueName,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Accuracy = source.Accuracy,
      Power = source.Power,
      PowerPoints = source.PowerPoints,
      InflictedStatus = source.GetInflictedStatus(),
      Link = source.Link,
      Notes = source.Notes
    };

    foreach (KeyValuePair<PokemonStatistic, int> statisticChange in source.GetStatisticChanges())
    {
      destination.StatisticChanges.Add(new StatisticChangeModel(statisticChange));
    }
    foreach (VolatileCondition volatileCondition in source.GetVolatileConditions())
    {
      destination.VolatileConditions.Add(volatileCondition.Value);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public RegionModel ToRegion(RegionEntity source)
  {
    RegionModel destination = new()
    {
      UniqueName = source.UniqueName,
      DisplayName = source.DisplayName,
      Description = source.Description,
      Link = source.Link,
      Notes = source.Notes
    };

    MapAggregate(source, destination);

    return destination;
  }

  private void MapAggregate(AggregateEntity source, AggregateModel destination)
  {
    destination.Id = new StreamId(source.StreamId).ToGuid();
    destination.Version = source.Version;
    destination.CreatedBy = TryFindActor(source.CreatedBy) ?? _system;
    destination.CreatedOn = source.CreatedOn.AsUniversalTime();
    destination.UpdatedBy = TryFindActor(source.UpdatedBy) ?? _system;
    destination.UpdatedOn = source.UpdatedOn.AsUniversalTime();
  }

  private ActorModel FindActor(string id) => FindActor(new ActorId(id));
  private ActorModel FindActor(ActorId id) => _actors.TryGetValue(id, out ActorModel? actor) ? actor : _system;
  private ActorModel? TryFindActor(string? id) => TryFindActor(id == null ? null : new ActorId(id));
  private ActorModel? TryFindActor(ActorId? id) => id.HasValue ? FindActor(id.Value) : null;
}
