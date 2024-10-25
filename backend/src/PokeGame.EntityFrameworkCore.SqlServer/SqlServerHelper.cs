using Logitar.Data;
using Logitar.Data.SqlServer;

namespace PokeGame.EntityFrameworkCore.SqlServer;

internal class SqlServerHelper : SqlHelper
{
  public override IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
