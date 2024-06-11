using Logitar.Data;

namespace PokeGame.EntityFrameworkCore;

public interface ISqlHelper
{
  IQueryBuilder QueryFrom(TableId table);
} // TODO(fpion): implement
