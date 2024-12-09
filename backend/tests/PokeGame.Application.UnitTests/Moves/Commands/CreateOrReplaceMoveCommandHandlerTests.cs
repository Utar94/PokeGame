﻿using FluentValidation;
using Logitar.Security.Cryptography;
using MediatR;
using Moq;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class CreateOrReplaceMoveCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();
  private readonly Mock<ISender> _sender = new();

  private readonly CreateOrReplaceMoveCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Move _move;

  public CreateOrReplaceMoveCommandHandlerTests()
  {
    _handler = new(_moveQuerier.Object, _moveRepository.Object, _sender.Object);

    _move = new(PokemonType.Normal, MoveCategory.Status, new UniqueName("growl"), _userId);
    _moveRepository.Setup(x => x.LoadAsync(_move.Id, _cancellationToken)).ReturnsAsync(_move);
  }

  [Fact(DisplayName = "It should create a new move.")]
  public async Task It_should_create_a_new_move()
  {
    CreateOrReplaceMovePayload payload = new("thunder-shock")
    {
      Type = PokemonType.Electric,
      Category = MoveCategory.Special,
      DisplayName = " Thunder Shock ",
      Description = "  The user attacks the target with a jolt of electricity. This may also leave the target with paralysis.  ",
      Accuracy = 95,
      Power = 40,
      PowerPoints = 30,
      Status = new InflictedConditionModel(StatusCondition.Paralysis, chance: 10),
      VolatileConditions = ["でんきショック"],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Thunder_Shock_(move)",
      Notes = "    "
    };
    CreateOrReplaceMoveCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    command.Contextualize();

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(It.IsAny<Move>(), _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Move);
    Assert.True(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveMoveCommand>(y => y.Move.Id.ToGuid() == command.Id && Comparisons.AreEqual(y.Move.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Move.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Move.Description, payload.Description)
        && y.Move.Type == payload.Type && y.Move.Category == payload.Category
        && y.Move.Accuracy == payload.Accuracy && y.Move.Power == payload.Power && y.Move.PowerPoints == payload.PowerPoints
        && y.Move.StatisticChanges.Count == 0
        && Comparisons.AreEqual(y.Move.Status, payload.Status)
        && y.Move.VolatileConditions.Single().Value == payload.VolatileConditions.Single()
        && Comparisons.AreEqual(y.Move.Link, payload.Link) && Comparisons.AreEqual(y.Move.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should replace an existing move.")]
  public async Task It_should_replace_an_existing_move()
  {
    _move.Status = new InflictedCondition(StatusCondition.Sleep, chance: 75);
    _move.AddVolatileCondition(new VolatileCondition("VC"));
    _move.SetStatisticChange(BattleStatistic.Speed, stages: -2);
    _move.Update(_userId);

    CreateOrReplaceMovePayload payload = new("Growl")
    {
      Type = PokemonType.Dragon,
      Category = MoveCategory.Special,
      DisplayName = " Growl ",
      Description = "  The user growls in an endearing way, making opposing Pokémon less wary. This lowers their Attack stats.  ",
      Accuracy = 100,
      PowerPoints = 40,
      StatisticChanges = [new StatisticChangeModel(BattleStatistic.Attack, stages: -1)],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Growl_(move)",
      Notes = "  Growl decreases the Attack stat of all adjacent opponents by one stage.\n\nGrowl is a sound-based move.  "
    };
    CreateOrReplaceMoveCommand command = new(_move.Id.ToGuid(), payload, Version: null);
    command.Contextualize();

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(_move, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Move);
    Assert.False(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveMoveCommand>(y => y.Move.Equals(_move) && Comparisons.AreEqual(y.Move.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Move.DisplayName, payload.DisplayName) && Comparisons.AreEqual(y.Move.Description, payload.Description)
        && y.Move.Type == _move.Type && y.Move.Category == _move.Category
        && y.Move.Accuracy == payload.Accuracy && y.Move.Power == payload.Power && y.Move.PowerPoints == payload.PowerPoints
        && y.Move.StatisticChanges.Count == 1 && y.Move.StatisticChanges.Single().Key == BattleStatistic.Attack && y.Move.StatisticChanges.Single().Value == -1
        && y.Move.Status == null && y.Move.VolatileConditions.Count == 0
        && Comparisons.AreEqual(y.Move.Link, payload.Link) && Comparisons.AreEqual(y.Move.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return empty when updating an move that does not exist.")]
  public async Task It_should_return_empty_when_updating_an_move_that_does_not_exist()
  {
    CreateOrReplaceMovePayload payload = new("Growl")
    {
      PowerPoints = 35
    };
    CreateOrReplaceMoveCommand command = new(Guid.NewGuid(), payload, Version: 1);
    command.Contextualize();

    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Null(result.Move);
    Assert.False(result.Created);

    _moveRepository.Verify(x => x.SaveAsync(It.IsAny<Move>(), _cancellationToken), Times.Never);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    CreateOrReplaceMovePayload payload = new()
    {
      Type = (PokemonType)(-1),
      Category = (MoveCategory)(-1),
      UniqueName = " Growl ",
      DisplayName = RandomStringGenerator.GetString(1000),
      Accuracy = 0,
      Power = 0,
      PowerPoints = 100,
      StatisticChanges = [new StatisticChangeModel((BattleStatistic)(-1), stages: -10)],
      Status = new InflictedConditionModel((StatusCondition)(-1), chance: 0),
      VolatileConditions = [RandomStringGenerator.GetString(1000)],
      Link = "test"
    };
    CreateOrReplaceMoveCommand command = new(Id: null, payload, Version: null);
    command.Contextualize();

    var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    Assert.Equal(13, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Type");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Category");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Accuracy");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Power");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "LessThanOrEqualValidator" && e.PropertyName == "PowerPoints");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "StatisticChanges[0].Statistic");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "StatisticChanges[0].Stages");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Status.Condition");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Status.Chance");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "VolatileConditions[0]");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  [Fact(DisplayName = "It should update an existing move.")]
  public async Task It_should_update_an_existing_move()
  {
    Move reference = new(_move.Type, _move.Category, _move.UniqueName, _userId, _move.Id);
    reference.AddVolatileCondition(new VolatileCondition("VC1"));
    reference.AddVolatileCondition(new VolatileCondition("VC2"));
    reference.SetStatisticChange(BattleStatistic.Attack, stages: -2);
    reference.SetStatisticChange(BattleStatistic.SpecialAttack, stages: -1);
    reference.Update(_userId);
    _moveRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

    Description description = new("The user growls in an endearing way, making opposing Pokémon less wary. This lowers their Attack stats.");
    _move.Description = description;
    _move.AddVolatileCondition(new VolatileCondition("VC1"));
    _move.AddVolatileCondition(new VolatileCondition("VC2"));
    _move.AddVolatileCondition(new VolatileCondition("VC3"));
    _move.SetStatisticChange(BattleStatistic.Attack, stages: -2);
    _move.SetStatisticChange(BattleStatistic.SpecialAttack, stages: -1);
    _move.SetStatisticChange(BattleStatistic.Evasion, stages: -3);
    _move.Update(_userId);

    CreateOrReplaceMovePayload payload = new("Growl")
    {
      Type = PokemonType.Dark,
      Category = MoveCategory.Physical,
      DisplayName = " Growl ",
      Description = "    ",
      Accuracy = 100,
      PowerPoints = 40,
      StatisticChanges =
      [
        new StatisticChangeModel(BattleStatistic.Attack, stages: -1),
        new StatisticChangeModel(BattleStatistic.Accuracy, stages: 3)
      ],
      Status = new InflictedConditionModel(StatusCondition.Poison, chance: 15),
      VolatileConditions = ["VC2", "VC4"],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Growl_(move)",
      Notes = "    "
    };
    CreateOrReplaceMoveCommand command = new(_move.Id.ToGuid(), payload, reference.Version);
    command.Contextualize();

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(_move, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Same(model, result.Move);
    Assert.False(result.Created);

    _sender.Verify(x => x.Send(
      It.Is<SaveMoveCommand>(y => y.Move.Equals(_move) && Comparisons.AreEqual(y.Move.UniqueName, payload.UniqueName)
        && Comparisons.AreEqual(y.Move.DisplayName, payload.DisplayName) && y.Move.Description == description
        && y.Move.Type == _move.Type && y.Move.Category == _move.Category
        && y.Move.StatisticChanges.Count == 3 && y.Move.StatisticChanges[BattleStatistic.Attack] == -1
        && y.Move.StatisticChanges[BattleStatistic.Evasion] == -3 && y.Move.StatisticChanges[BattleStatistic.Accuracy] == 3
        && Comparisons.AreEqual(y.Move.Status, payload.Status)
        && y.Move.VolatileConditions.Count == 3 && y.Move.VolatileConditions.Any(x => x.Value == "VC2")
        && y.Move.VolatileConditions.Any(x => x.Value == "VC3") && y.Move.VolatileConditions.Any(x => x.Value == "VC4")
        && Comparisons.AreEqual(y.Move.Link, payload.Link) && Comparisons.AreEqual(y.Move.Notes, payload.Notes)),
      _cancellationToken), Times.Once);
  }
}
