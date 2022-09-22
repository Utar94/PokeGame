namespace PokeGame.Domain.Pokemon
{
  public static class ExperienceTable
  {
    public static uint GetTotalExperience(LevelingRate levelingRate, byte level)
    {
      if (level <= 1 || level > 100)
      {
        return 0;
      }

      return levelingRate switch
      {
        LevelingRate.Erratic => GetTotalExperienceErratic(level),
        LevelingRate.Fast => (uint)(0.8 * Math.Pow(level, 3)),
        LevelingRate.MediumFast => (uint)Math.Pow(level, 3),
        LevelingRate.MediumSlow => (uint)((1.2 * Math.Pow(level, 3)) - (15.0 * Math.Pow(level, 2)) + (100.0 * level) - 140.0),
        LevelingRate.Slow => (uint)(1.25 * Math.Pow(level, 3)),
        LevelingRate.Fluctuating => GetTotalExperienceFluctuating(level),
        _ => throw new NotSupportedException($"The leveling rate '{levelingRate}' is not supported."),
      };
    }

    private static uint GetTotalExperienceErratic(byte level)
    {
      if (level < 50)
      {
        return (uint)(Math.Pow(level, 3) * (100.0 - level) / 50.0);
      }
      else if (level < 68)
      {
        return (uint)(Math.Pow(level, 3) * (150.0 - level) / 100.0);
      }
      else if (level < 98)
      {
        return (uint)(Math.Pow(level, 3) * Math.Floor((1911.0 - (level * 10.0)) / 3.0) / 500.0);
      }
      else
      {
        return (uint)(Math.Pow(level, 3) * (160.0 - level) / 100.0);
      }
    }

    private static uint GetTotalExperienceFluctuating(byte level)
    {
      if (level < 15)
      {
        return (uint)(Math.Pow(level, 3) * (Math.Floor((level + 1.0) / 3.0) + 24.0) / 50.0);
      }
      else if (level < 36)
      {
        return (uint)(Math.Pow(level, 3) * ((level + 14.0) / 50.0));
      }
      else
      {
        return (uint)(Math.Pow(level, 3) * ((Math.Floor(level / 2.0) + 32.0) / 50.0));
      }
    }
  }
}
