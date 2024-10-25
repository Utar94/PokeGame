using MediatR;
using PokeGame.Infrastructure.Commands;

namespace PokeGame.Seeding.Worker.Backend.Tasks;

internal class MigrateDatabaseTask : SeedingTask
{
  public override string? Description => "Applies database migrations.";
}

internal class MigrateDatabaseTaskHandler : INotificationHandler<MigrateDatabaseTask>
{
  private readonly IPublisher _publisher;

  public MigrateDatabaseTaskHandler(IPublisher publisher)
  {
    _publisher = publisher;
  }

  public async Task Handle(MigrateDatabaseTask notification, CancellationToken cancellationToken)
  {
    await _publisher.Publish(new InitializeDatabaseCommand(), cancellationToken);
  }
}
