using Logitar.Data;
using Logitar.Data.SqlServer;

namespace PokeGame.Infrastructure.SqlServer;

internal class SqlServerHelper : SqlHelper
{
  public override IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
