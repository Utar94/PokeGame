namespace PokeGame.Application.Logging;

internal class FakeLogRepository : ILogRepository
{
  public Task SaveAsync(Log log, CancellationToken cancellationToken) => Task.CompletedTask;
}
