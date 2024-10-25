using MediatR;

namespace PokeGame.Infrastructure.Commands;

public record InitializeDatabaseCommand : INotification;
