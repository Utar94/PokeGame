using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Web.Models.Api.Game
{
  public class PokemonSummary
  {
    public PokemonSummary(PokemonModel pokemon)
    {
      HistoryModel history = pokemon.History ?? throw new ArgumentException($"The {nameof(pokemon.History)} is required.", nameof(pokemon));
      TrainerModel trainer = history.Trainer ?? throw new ArgumentException($"The {nameof(pokemon.History)}.{nameof(history.Trainer)} is required.", nameof(pokemon));

      SpeciesModel species = pokemon.Species ?? throw new ArgumentException($"The {nameof(pokemon.Species)} is required.", nameof(pokemon));

      IsEgg = pokemon.RemainingHatchSteps > 0;

      if (IsEgg)
      {
        RemainingEggCycles = (byte)(pokemon.RemainingHatchSteps / 257);
      }
      else
      {
        Number = trainer.NationalPokedex
          ? species.Number
          : species.RegionalNumbers.SingleOrDefault(x => x.Region == trainer.LegacyRegion)?.Number;
        Name = pokemon.Surname ?? species.Name;

        PrimaryType = species.PrimaryType;
        SecondaryType = species.SecondaryType;

        CurrentExperience = pokemon.Experience;
        ExperienceToNextLevel = pokemon.ExperienceToNextLevel;

        HeldItem = pokemon.HeldItem == null ? null : new(pokemon.HeldItem);
        OriginalTrainer = pokemon.OriginalTrainer == null ? null : new(pokemon.OriginalTrainer);

        Level = pokemon.Level;
        Gender = pokemon.Gender;
        Nature = new NatureSummary(pokemon.Nature);
        Characteristic = pokemon.Characteristic;

        MetLevel = history.Level;
        MetLocation = history.Location;

        CurrentHitPoints = pokemon.CurrentHitPoints;
        MaximumHitPoints = pokemon.MaximumHitPoints;
        Attack = pokemon.Attack;
        Defense = pokemon.Defense;
        SpecialAttack = pokemon.SpecialAttack;
        SpecialDefense = pokemon.SpecialDefense;
        Speed = pokemon.Speed;

        Ability = pokemon.Ability == null ? null : new(pokemon.Ability);
        Moves = pokemon.Moves.Select(move => new PokemonMoveSummary(move));

        Picture = species.Picture;
      }

      CaughtBall = history.Ball == null ? null : new(history.Ball);

      MetOn = history.MetOn;
    }

    public bool IsEgg { get; set; }
    public byte RemainingEggCycles { get; set; }

    public int? Number { get; set; }
    public string? Name { get; set; }

    public PokemonType? PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public int? CurrentExperience { get; set; }
    public int? ExperienceToNextLevel { get; set; }

    public ItemSummary? CaughtBall { get; set; }
    public ItemSummary? HeldItem { get; set; }
    public TrainerSummary? OriginalTrainer { get; set; }

    public byte? Level { get; set; }
    public PokemonGender? Gender { get; set; }
    public NatureSummary? Nature { get; set; }
    public Characteristic? Characteristic { get; set; }

    public byte? MetLevel { get; set; }
    public string? MetLocation { get; set; }
    public DateTime MetOn { get; set; }

    public ushort? CurrentHitPoints { get; set; }
    public ushort? MaximumHitPoints { get; set; }
    public ushort? Attack { get; set; }
    public ushort? Defense { get; set; }
    public ushort? SpecialAttack { get; set; }
    public ushort? SpecialDefense { get; set; }
    public ushort? Speed { get; set; }

    public AbilitySummary? Ability { get; set; }
    public IEnumerable<PokemonMoveSummary>? Moves { get; set; }

    public string? Picture { get; set; }
  }
}
