namespace PokeGame.EntityFrameworkCore.Entities;

internal abstract class EnumEntity
{
  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;

  protected EnumEntity()
  {
  }

  protected EnumEntity(object value)
  {
    Id = (int)value;
    Name = value.ToString() ?? string.Empty;
  }

  public override bool Equals(object? obj) => obj is EnumEntity entity && entity.GetType().Equals(GetType()) && entity.Id == Id;
  public override int GetHashCode() => HashCode.Combine(GetType(), Id);
  public override string ToString() => $"{Name} | {GetType()} (Id={Id})";
}
