﻿using PokeGame.Application.Models;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Models
{
  public class TrainerModel : AggregateModel
  {
    public ActorModel? User { get; set; }

    public RegionModel? Region { get; set; }
    public int Number { get; set; }

    public int Money { get; set; }
    public int PlayTime { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Picture { get; set; }
    public string? Reference { get; set; }

    public bool NationalPokedex { get; private set; }
    public int PokedexCount { get; private set; }
  }
}
