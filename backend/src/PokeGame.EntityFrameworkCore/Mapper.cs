using Logitar;
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
  private readonly Dictionary<ActorId, Actor> _actors = [];
  private readonly Actor _system = Actor.System;

  public Mapper()
  {
  }

  public Mapper(IEnumerable<Actor> actors) : this()
  {
    foreach (Actor actor in actors)
    {
      ActorId id = new(actor.Id);
      _actors[id] = actor;
    }
  }

  public static Actor ToActor(UserEntity user) => new()
  {
    Id = user.Id,
    Type = ActorType.User,
    DisplayName = user.DisplayName,
    EmailAddress = user.EmailAddress,
    PictureUrl = user.PictureUrl
  };

  public AbilityModel ToAbility(AbilityEntity source)
  {
    AbilityModel destination = new()
    {
      Id = source.Id,
      Kind = source.Kind,
      Name = source.Name,
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
      Id = source.Id,
      Type = source.Type,
      Category = source.Category,
      Kind = source.Kind,
      Name = source.Name,
      Description = source.Description,
      Accuracy = source.Accuracy,
      Power = source.Power,
      PowerPoints = source.PowerPoints,
      Status = source.GetInflictedStatus(),
      Link = source.Link,
      Notes = source.Notes
    };

    destination.StatisticChanges.AddRange(source.GetStatisticChanges());
    destination.VolatileConditions.AddRange(source.GetVolatileConditions());

    MapAggregate(source, destination);

    return destination;
  }

  public RegionModel ToRegion(RegionEntity source)
  {
    RegionModel destination = new()
    {
      Id = source.Id,
      Name = source.Name,
      Description = source.Description,
      Link = source.Link,
      Notes = source.Notes
    };

    MapAggregate(source, destination);

    return destination;
  }

  private void MapAggregate(AggregateEntity source, Aggregate destination)
  {
    destination.Version = source.Version;
    destination.CreatedBy = FindActor(source.CreatedBy);
    destination.CreatedOn = source.CreatedOn.AsUniversalTime();
    destination.UpdatedBy = FindActor(source.UpdatedBy);
    destination.UpdatedOn = source.UpdatedOn;
  }

  private Actor FindActor(string id) => FindActor(new ActorId(id));
  private Actor FindActor(ActorId id) => _actors.TryGetValue(id, out Actor? actor) ? actor : _system;
}
