namespace PokeGame.Domain.Pokemon
{
  public sealed class Nature
  {
    private static readonly Dictionary<string, Nature> _natures = new()
    {
      ["Adamant"] = new("Adamant", Statistic.Attack, Statistic.SpecialAttack),
      ["Bashful"] = new("Bashful"),
      ["Bold"] = new("Bold", Statistic.Defense, Statistic.Attack),
      ["Brave"] = new("Brave", Statistic.Attack, Statistic.Speed),
      ["Calm"] = new("Calm", Statistic.SpecialDefense, Statistic.Attack),
      ["Careful"] = new("Careful", Statistic.SpecialDefense, Statistic.SpecialAttack),
      ["Docile"] = new("Docile"),
      ["Gentle"] = new("Gentle", Statistic.SpecialDefense, Statistic.Defense),
      ["Hardy"] = new("Hardy"),
      ["Hasty"] = new("Hasty", Statistic.Speed, Statistic.Defense),
      ["Impish"] = new("Impish", Statistic.Defense, Statistic.SpecialAttack),
      ["Jolly"] = new("Jolly", Statistic.Speed, Statistic.SpecialAttack),
      ["Lax"] = new("Lax", Statistic.Defense, Statistic.SpecialDefense),
      ["Lonely"] = new("Lonely", Statistic.Attack, Statistic.Defense),
      ["Mild"] = new("Mild", Statistic.SpecialAttack, Statistic.Defense),
      ["Modest"] = new("Modest", Statistic.SpecialAttack, Statistic.Attack),
      ["Naive"] = new("Naive", Statistic.Speed, Statistic.SpecialDefense),
      ["Naughty"] = new("Naughty", Statistic.Attack, Statistic.SpecialDefense),
      ["Quiet"] = new("Quiet", Statistic.SpecialAttack, Statistic.Speed),
      ["Quirky"] = new("Quirky"),
      ["Rash"] = new("Rash", Statistic.SpecialAttack, Statistic.SpecialDefense),
      ["Relaxed"] = new("Relaxed", Statistic.Defense, Statistic.Speed),
      ["Sassy"] = new("Sassy", Statistic.SpecialDefense, Statistic.Speed),
      ["Serious"] = new("Serious"),
      ["Timid"] = new("Timid", Statistic.Speed, Statistic.Attack),
    };

    private Nature(string name)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
    }
    private Nature(string name, Statistic increasedStatistic, Statistic decreasedStatistic)
      : this(name)
    {
      IncreasedStatistic = increasedStatistic;
      DecreasedStatistic = decreasedStatistic;
    }

    public string Name { get; }

    public Statistic? IncreasedStatistic { get; }
    public Statistic? DecreasedStatistic { get; }

    public Flavor? FavoriteFlavor => IncreasedStatistic.HasValue ? GetFlavor(IncreasedStatistic.Value) : null;
    public Flavor? DislikedFlavor => DecreasedStatistic.HasValue ? GetFlavor(DecreasedStatistic.Value) : null;

    public static Nature GetNature(string name, string? paramName = null)
    {
      if (!_natures.TryGetValue(name, out Nature? nature))
      {
        throw new NatureNotFoundException(name, paramName);
      }

      return nature;
    }

    private static Flavor GetFlavor(Statistic statistic)
    {
      return statistic switch
      {
        Statistic.Attack => Flavor.Spicy,
        Statistic.Defense => Flavor.Sour,
        Statistic.SpecialAttack => Flavor.Dry,
        Statistic.SpecialDefense => Flavor.Bitter,
        Statistic.Speed => Flavor.Sweet,
        _ => throw new ArgumentException($"The statistic '{statistic}' does not resolve to a flavor.", nameof(statistic)),
      };
    }

    public override bool Equals(object? obj) => obj is Nature nature && nature.Name == Name;
    public override int GetHashCode() => HashCode.Combine(GetType(), Name);
    public override string ToString() => Name;
  }
}
