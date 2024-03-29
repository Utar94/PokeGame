﻿using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonCaught : DomainEvent, INotification
  {
    public PokemonCaught(Guid ballId, string location, Guid trainerId, byte position, byte? box = null, byte? friendship = null, string? surname = null)
    {
      BallId = ballId;
      Box = box;
      Location = location;
      Position = position;
      Friendship = friendship;
      Surname = surname;
      TrainerId = trainerId;
    }

    public byte? Friendship { get; private set; }
    public string? Surname { get; private set; }

    public Guid BallId { get; private set; }
    public string Location { get; private set; }
    public Guid TrainerId { get; private set; }

    public byte Position { get; private set; }
    public byte? Box { get; private set; }
  }
}
