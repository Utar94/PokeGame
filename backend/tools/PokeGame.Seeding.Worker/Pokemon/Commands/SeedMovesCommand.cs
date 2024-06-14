using MediatR;

namespace PokeGame.Seeding.Worker.Pokemon.Commands;

internal record SeedMovesCommand : INotification;
