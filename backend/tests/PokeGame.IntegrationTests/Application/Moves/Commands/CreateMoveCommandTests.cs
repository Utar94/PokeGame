using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class CreateMoveCommandTests : IntegrationTests
{
  private readonly IMoveRepository _moveRepository;

  public CreateMoveCommandTests() : base()
  {
    _moveRepository = ServiceProvider.GetRequiredService<IMoveRepository>();
  }

  [Fact(DisplayName = "It should create a new move.")]
  public async Task It_should_create_a_new_move()
  {
    CreateMovePayload payload = new("Tackle")
    {
      Type = PokemonType.Normal,
      Category = MoveCategory.Physical,
      Accuracy = 100,
      Power = 40,
      PowerPoints = 35
    };
    CreateMoveCommand command = new(payload);
    Move move = await Pipeline.ExecuteAsync(command);

    Assert.NotEqual(Guid.Empty, move.Id);
    Assert.Equal(2, move.Version);
    Assert.Equal(Actor, move.CreatedBy);
    Assert.Equal(Actor, move.UpdatedBy);
    Assert.True(move.CreatedOn < move.UpdatedOn);

    Assert.Equal(payload.Type, move.Type);
    Assert.Equal(payload.Category, move.Category);
    Assert.Equal(payload.UniqueName, move.UniqueName);
    Assert.Equal(payload.Accuracy, move.Accuracy);
    Assert.Equal(payload.Power, move.Power);
    Assert.Equal(payload.PowerPoints, move.PowerPoints);

    MoveEntity? entity = await PokemonContext.Moves.AsNoTracking().SingleOrDefaultAsync(x => x.AggregateId == new AggregateId(move.Id).Value);
    Assert.NotNull(entity);
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    MoveAggregate move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueNameUnit(MoveAggregate.UniqueNameSettings, "Tackle"));
    await _moveRepository.SaveAsync(move);

    CreateMovePayload payload = new(move.UniqueName.Value)
    {
      Type = PokemonType.Normal,
      Category = MoveCategory.Physical,
      Accuracy = 100,
      Power = 40,
      PowerPoints = 35
    };
    CreateMoveCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<MoveAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateMovePayload payload = new("Razor Leaf")
    {
      Type = PokemonType.Grass,
      Category = MoveCategory.Physical,
      Accuracy = 95,
      Power = 55,
      PowerPoints = 25
    };
    CreateMoveCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal(nameof(AllowedCharactersValidator), error.ErrorCode);
    Assert.Equal("UniqueName", error.PropertyName);
  }
}
