namespace PokeGame.Application.Models
{
  public class ListModel<T>
  {
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public long Total { get; set; }
  }
}
