using FluentValidation;
using Logitar.Security.Cryptography;
using MediatR;
using Moq;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateMoveCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();
  private readonly Mock<ISender> _sender = new();

  private readonly UpdateMoveCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Move _move;

  public UpdateMoveCommandHandlerTests()
  {
    _handler = new(_moveQuerier.Object, _moveRepository.Object, _sender.Object);

    _move = new(PokemonType.Normal, MoveCategory.Status, new UniqueName("growl"), _userId);
    _moveRepository.Setup(x => x.LoadAsync(_move.Id, _cancellationToken)).ReturnsAsync(_move);
  }

  [Fact(DisplayName = "It should return null when the move could not be found.")]
  public async Task It_should_return_null_when_the_move_could_not_be_found()
  {
    UpdateMovePayload payload = new();
    UpdateMoveCommand command = new(Guid.NewGuid(), payload);
    command.Contextualize();

    Assert.Null(await _handler.Handle(command, _cancellationToken));
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    UpdateMovePayload payload = new()
    {
      UniqueName = " Growl ",
      DisplayName = new Change<string>(RandomStringGenerator.GetString(1000)),
      Accuracy = new Change<int?>(10000),
      Power = new Change<int?>(1000),
      PowerPoints = 100,
      StatisticChanges = [new StatisticChangeModel(BattleStatistic.Accuracy, stages: 10)],
      Status = new Change<InflictedConditionModel>(new InflictedConditionModel(StatusCondition.Freeze, chance: -10)),
      VolatileConditions =
      [
        new VolatileConditionUpdate(RandomStringGenerator.GetString(1000), (ActionKind)(-1))
      ],
      Link = new Change<string>("test")
    };
    UpdateMoveCommand command = new(_move.Id.ToGuid(), payload);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(10, exception.Errors.Count());

    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "LessThanOrEqualValidator" && e.PropertyName == "Accuracy.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "LessThanOrEqualValidator" && e.PropertyName == "Power.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "LessThanOrEqualValidator" && e.PropertyName == "PowerPoints");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "StatisticChanges[0].Stages");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Status.Value.Chance");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "VolatileConditions[0].VolatileCondition");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "VolatileConditions[0].Action");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link.Value");
  }

  [Fact(DisplayName = "It should update an existing move.")]
  public async Task It_should_update_an_existing_move()
  {
    _move.SetStatisticChange(BattleStatistic.Attack, stages: -2);
    _move.SetStatisticChange(BattleStatistic.SpecialAttack, stages: -2);
    _move.Status = new InflictedCondition(StatusCondition.Burn, chance: 10);
    _move.AddVolatileCondition(new VolatileCondition("Attack -1"));
    _move.AddVolatileCondition(new VolatileCondition("SpecialAttack -1"));

    Description description = new("The user growls in an endearing way, making opposing Pokémon less wary. This lowers their Attack stats.");
    _move.Description = description;
    _move.Update(_userId);

    UpdateMovePayload payload = new()
    {
      UniqueName = "Growl",
      DisplayName = new Change<string>(" Growl "),
      Accuracy = new Change<int?>(100),
      PowerPoints = 40,
      StatisticChanges = [new StatisticChangeModel(BattleStatistic.Attack, stages: -1)],
      Status = new Change<InflictedConditionModel>(new InflictedConditionModel(StatusCondition.Poison, chance: 50)),
      VolatileConditions =
      [
        new VolatileConditionUpdate("Growl", ActionKind.Add),
        new VolatileConditionUpdate("SpecialAttack -1", ActionKind.Remove)
      ],
      Link = new Change<string>("https://bulbapedia.bulbagarden.net/wiki/Growl_(move)"),
      Notes = new Change<string>("Growl decreases the Attack stat of all adjacent opponents by one stage.\n\nGrowl is a sound-based move.")
    };
    UpdateMoveCommand command = new(_move.Id.ToGuid(), payload);
    command.Contextualize();

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(_move, _cancellationToken)).ReturnsAsync(model);

    MoveModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _sender.Verify(x => x.Send(
      It.Is<SaveMoveCommand>(y => y.Move.Equals(_move) && Comparisons.AreEqual(y.Move.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Move.DisplayName, payload.DisplayName.Value) && y.Move.Description == description
        && y.Move.Accuracy == payload.Accuracy.Value && y.Move.Power == null && y.Move.PowerPoints == payload.PowerPoints
        && y.Move.StatisticChanges.Count == 2 && y.Move.StatisticChanges[BattleStatistic.Attack] == -1 && y.Move.StatisticChanges[BattleStatistic.SpecialAttack] == -2
        && Comparisons.AreEqual(y.Move.Status, payload.Status.Value)
        && y.Move.VolatileConditions.Count == 2 && y.Move.VolatileConditions.Any(y => y.Value == "Attack -1") && y.Move.VolatileConditions.Any(y => y.Value == "Growl")
        && Comparisons.AreEqual(y.Move.Link, payload.Link.Value) && Comparisons.AreEqual(y.Move.Notes, payload.Notes.Value)),
      _cancellationToken), Times.Once);
  }
}
