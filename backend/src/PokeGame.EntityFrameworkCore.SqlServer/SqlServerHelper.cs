using Logitar.Data;
using Logitar.Data.SqlServer;

namespace PokeGame.EntityFrameworkCore.SqlServer;

internal class SqlServerHelper : ISqlHelper
{
  public IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
