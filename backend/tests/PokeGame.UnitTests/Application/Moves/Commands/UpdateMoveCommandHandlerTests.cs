using Bogus;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Moq;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateMoveCommandHandlerTests
{
  private readonly ActorId _actorId = ActorId.NewId();
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IApplicationContext> _applicationContext = new();
  private readonly Mock<IMoveManager> _moveManager = new();
  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();

  private readonly UpdateMoveCommandHandler _handler;

  public UpdateMoveCommandHandlerTests()
  {
    _handler = new(_applicationContext.Object, _moveManager.Object, _moveQuerier.Object, _moveRepository.Object);

    _applicationContext.Setup(x => x.GetActorId()).Returns(_actorId);
  }

  [Fact(DisplayName = "It should return null when the move was not found.")]
  public async Task Given_NotFound_When_Handle_Then_NullReturned()
  {
    UpdateMovePayload payload = new();
    UpdateMoveCommand command = new(Guid.NewGuid(), payload);
    MoveModel? move = await _handler.Handle(command, _cancellationToken);
    Assert.Null(move);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task Given_InvalidPayload_When_Handle_Then_ValidationException()
  {
    UpdateMovePayload payload = new()
    {
      DisplayName = new ChangeModel<string>(_faker.Random.String(999, minChar: 'A', maxChar: 'Z')),
      Accuracy = new ChangeModel<int?>(0),
      Power = new ChangeModel<int?>(1000),
      PowerPoints = 100,
      InflictedStatus = new ChangeModel<InflictedStatusModel>(new InflictedStatusModel((StatusCondition)(-1), chance: 0)),
      StatisticChanges = [new StatisticChangeModel(PokemonStatistic.HP, stages: 10)],
      VolatileConditions = [new VolatileConditionAction(string.Empty, (CollectionAction)(-1))],
      Link = new ChangeModel<string>("leer")
    };
    UpdateMoveCommand command = new(Guid.NewGuid(), payload);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));

    Assert.Equal(11, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "Accuracy.Value.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "Power.Value.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "PowerPoints.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "InflictedStatus.Value.Condition");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "InflictedStatus.Value.Chance");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEqualValidator" && e.PropertyName == "StatisticChanges[0].Statistic");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "StatisticChanges[0].Stages");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "VolatileConditions[0].Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "VolatileConditions[0].Action");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link.Value");
  }

  [Fact(DisplayName = "It should update an existing move.")]
  public async Task Given_Exists_When_Handle_Then_Updated()
  {
    Move move = new(PokemonType.Fire, MoveCategory.Special, new UniqueName("ember"), new PowerPoints(5))
    {
      Notes = new Notes("Ember deals damage and has a 10% chance of burning the target.")
    };
    move.SetStatisticChange(PokemonStatistic.Attack, stages: -1);
    move.SetVolatileConditions([new VolatileCondition("ember")]);
    _moveRepository.Setup(x => x.LoadAsync(move.Id, _cancellationToken)).ReturnsAsync(move);

    DisplayName displayName = new("Ember");
    move.DisplayName = displayName;
    move.Update();

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(move, _cancellationToken)).ReturnsAsync(model);

    UpdateMovePayload payload = new()
    {
      UniqueName = "Ember",
      DisplayName = null,
      Description = new ChangeModel<string>("  The target is attacked with small flames. This may also leave the target with a burn.  "),
      Accuracy = new ChangeModel<int?>(100),
      Power = new ChangeModel<int?>(40),
      PowerPoints = 25,
      InflictedStatus = new ChangeModel<InflictedStatusModel>(new InflictedStatusModel(StatusCondition.Burn, chance: 10)),
      StatisticChanges = [new StatisticChangeModel(PokemonStatistic.Attack, stages: 0)],
      VolatileConditions = [new VolatileConditionAction("ember", CollectionAction.Remove), new VolatileConditionAction("Ember", CollectionAction.Add)],
      Link = new ChangeModel<string>("https://bulbapedia.bulbagarden.net/wiki/Ember_(move)"),
      Notes = new ChangeModel<string>("    ")
    };
    UpdateMoveCommand command = new(move.Id.ToGuid(), payload);
    MoveModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _moveManager.Verify(x => x.SaveAsync(move, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, move.UpdatedBy);
    Assertions.Equal(payload.UniqueName, move.UniqueName);
    Assert.Equal(displayName, move.DisplayName);
    Assertions.Equal(payload.Description.Value, move.Description);
    Assertions.Equal(payload.Accuracy.Value, move.Accuracy);
    Assertions.Equal(payload.Power.Value, move.Power);
    Assertions.Equal(payload.PowerPoints.Value, move.PowerPoints);
    Assertions.Equal(payload.InflictedStatus.Value, move.InflictedStatus);
    Assert.Empty(move.StatisticChanges);
    Assert.Equal("Ember", Assert.Single(move.VolatileConditions).Value);
    Assertions.Equal(payload.Link.Value, move.Link);
    Assertions.Equal(payload.Notes.Value, move.Notes);
  }
}
