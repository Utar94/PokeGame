using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;

namespace PokeGame.EntityFrameworkCore;

internal static class QueryingExtensions
{
  public static IQueryBuilder ApplyIdInFilter(this IQueryBuilder builder, ColumnId column, SearchPayload payload)
  {
    if (payload.Ids.Count < 1)
    {
      return builder;
    }

    string[] aggregateIds = payload.Ids.Distinct().Select(id => new AggregateId(id).Value).ToArray();

    return builder.Where(column, Operators.IsIn(aggregateIds));
  }

  public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, SearchPayload payload)
  {
    if (payload.Skip > 0)
    {
      query = query.Skip(payload.Skip);
    }

    if (payload.Limit > 0)
    {
      query = query.Take(payload.Limit);
    }

    return query;
  }

  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQueryBuilder query) where T : class
  {
    return entities.FromQuery(query.Build());
  }
  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQuery query) where T : class
  {
    return entities.FromSqlRaw(query.Text, query.Parameters.ToArray());
  }
}
