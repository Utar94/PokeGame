namespace PokeGame.Domain.Pokemon
{
  internal static class PokemonExtensions
  {
    public static ushort CalculateStatistic(this Pokemon pokemon, Statistic statistic)
    {
      ArgumentNullException.ThrowIfNull(pokemon);

      byte level = pokemon.Level;
      Nature nature = pokemon.Nature;

      pokemon.BaseStatistics.TryGetValue(statistic, out byte baseValue);
      pokemon.IndividualValues.TryGetValue(statistic, out byte individualValue);
      pokemon.EffortValues.TryGetValue(statistic, out byte effortValue);

      int value = ((2 * baseValue + individualValue + (effortValue / 4)) * level / 100) + 5;

      if (statistic == Statistic.HP)
      {
        return (ushort)(value + level + 5);
      }

      double? modifier = nature.IncreasedStatistic == statistic ? 1.1 : (nature.DecreasedStatistic == statistic ? 0.9 : null);
      if (modifier.HasValue)
      {
        value = (int)(value * modifier.Value);
      }

      return (ushort)value;
    }
  }
}
