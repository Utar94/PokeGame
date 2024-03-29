﻿using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class HealPokemonMutation : IRequest<PokemonModel>
  {
    public HealPokemonMutation(Guid id, HealPokemonPayload payload)
    {
      Id = id;
      Payload = payload;
    }

    public Guid Id { get; }
    public HealPokemonPayload Payload { get; }
  }
}
