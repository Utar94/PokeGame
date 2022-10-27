namespace PokeGame.ReadModel.Entities.Enums
{
  internal abstract class EnumEntity
  {
    protected EnumEntity(object value)
    {
      Id = (int)value;
      Name = value.ToString() ?? string.Empty;
    }
    protected EnumEntity()
    {
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
  }
}
