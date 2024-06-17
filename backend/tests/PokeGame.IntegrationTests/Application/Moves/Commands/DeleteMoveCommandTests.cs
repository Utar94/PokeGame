using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class DeleteMoveCommandTests : IntegrationTests
{
  private readonly IMoveRepository _moveRepository;

  private readonly MoveAggregate _move;

  public DeleteMoveCommandTests()
  {
    _moveRepository = ServiceProvider.GetRequiredService<IMoveRepository>();

    _move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueNameUnit(MoveAggregate.UniqueNameSettings, "Tackle"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _moveRepository.SaveAsync(_move);
  }

  [Fact(DisplayName = "It should delete an existing move.")]
  public async Task It_should_delete_an_existing_move()
  {
    DeleteMoveCommand command = new(_move.Id.ToGuid());
    Move? move = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(move);
    Assert.Equal(_move.Id.ToGuid(), move.Id);

    MoveEntity? entity = await PokemonContext.Moves.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _move.Id.Value);
    Assert.Null(entity);
  }

  [Fact(DisplayName = "It should return null when the move could not be found.")]
  public async Task It_should_return_null_when_the_move_could_not_be_found()
  {
    DeleteMoveCommand command = new(Guid.NewGuid());
    Move? move = await Pipeline.ExecuteAsync(command);
    Assert.Null(move);
  }
}
