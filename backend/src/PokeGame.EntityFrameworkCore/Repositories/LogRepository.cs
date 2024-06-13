using Logitar;
using PokeGame.Application.Logging;
using PokeGame.EntityFrameworkCore.Entities;
using PokeGame.Infrastructure;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class LogRepository : ILogRepository
{
  private readonly PokemonContext _context;
  private readonly JsonSerializerOptions _serializerOptions = new();

  public LogRepository(PokemonContext context, IServiceProvider serviceProvider)
  {
    _context = context;
    _serializerOptions.Converters.AddRange(serviceProvider.GetJsonConverters());
  }

  public async Task SaveAsync(Log log, CancellationToken cancellationToken)
  {
    LogEntity entity = new(log, _serializerOptions);

    _context.Logs.Add(entity);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
