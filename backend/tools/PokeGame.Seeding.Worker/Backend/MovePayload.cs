using PokeGame.Contracts.Moves;

namespace PokeGame.Seeding.Worker.Backend;

internal record MovePayload : CreateOrReplaceMovePayload
{
  public Guid Id { get; set; }
}
