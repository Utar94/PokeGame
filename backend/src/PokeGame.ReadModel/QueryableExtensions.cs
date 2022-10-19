namespace PokeGame.ReadModel
{
  internal static class QueryableExtensions
  {
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int? index, int? count)
    {
      if (index.HasValue)
      {
        query = query.Skip(index.Value);
      }
      if (count.HasValue)
      {
        query = query.Take(count.Value);
      }

      return query;
    }
  }
}
