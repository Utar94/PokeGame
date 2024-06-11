using Logitar.Data;
using Logitar.Portal.Contracts.Search;

namespace PokeGame.EntityFrameworkCore;

public interface ISqlHelper
{
  IQueryBuilder ApplyTextSearch(IQueryBuilder query, TextSearch? search, params ColumnId[] columns);
  IQueryBuilder QueryFrom(TableId table);
}
