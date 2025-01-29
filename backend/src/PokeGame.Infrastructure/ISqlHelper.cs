using Logitar.Data;
using Logitar.Portal.Contracts.Search;

namespace PokeGame.Infrastructure;

public interface ISqlHelper
{
  void ApplyTextSearch(IQueryBuilder query, TextSearch search, params ColumnId[] columns);

  IQueryBuilder QueryFrom(TableId table);
}
