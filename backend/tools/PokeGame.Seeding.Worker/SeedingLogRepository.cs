using PokeGame.Application.Logging;

namespace PokeGame.Seeding.Worker;

internal class SeedingLogRepository : ILogRepository
{
  public Task SaveAsync(Log log, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
