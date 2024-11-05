using Logitar.Data;
using Logitar.Data.PostgreSQL;

namespace PokeGame.EntityFrameworkCore.PostgreSQL;

internal class PostgreSQLHelper : SqlHelper
{
  public override IQueryBuilder QueryFrom(TableId table) => PostgresQueryBuilder.From(table);
}
