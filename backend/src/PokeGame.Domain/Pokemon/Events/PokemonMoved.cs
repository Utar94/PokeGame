﻿using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonMoved : DomainEvent, INotification
  {
    public PokemonMoved(byte? position, byte? box)
    {
      Position = position;
      Box = box;
    }

    public byte? Position { get; private set; }
    public byte? Box { get; private set; }
  }
}
