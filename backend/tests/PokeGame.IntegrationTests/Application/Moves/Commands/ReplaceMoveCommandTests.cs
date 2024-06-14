using FluentValidation.Results;
using Logitar;
using Logitar.Identity.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Integration)]
public class ReplaceMoveCommandTests : IntegrationTests
{
  private readonly IMoveRepository _moveRepository;

  private readonly MoveAggregate _move;

  public ReplaceMoveCommandTests() : base()
  {
    _moveRepository = ServiceProvider.GetRequiredService<IMoveRepository>();

    _move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueNameUnit(MoveAggregate.UniqueNameSettings, "Tackle"), ActorId);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _moveRepository.SaveAsync(_move);
  }

  [Fact(DisplayName = "It should replace an existing move.")]
  public async Task It_should_replace_an_existing_move()
  {
    _move.SetStatisticChange(PokemonStatistic.Speed, -2);
    _move.SetStatisticChange(PokemonStatistic.SpecialAttack, -2);
    _move.SetStatusCondition(new StatusCondition("Burn"), 100);
    _move.SetStatusCondition(new StatusCondition("Poison"), 100);
    _move.Update(ActorId);
    long version = _move.Version;

    _move.Description = new DescriptionUnit("A physical attack in which the user charges and slams into the target with its whole body.");
    _move.SetStatisticChange(PokemonStatistic.Attack, -1);
    _move.SetStatisticChange(PokemonStatistic.SpecialAttack, -1);
    _move.RemoveStatisticChange(PokemonStatistic.Speed);
    _move.SetStatusCondition(new StatusCondition("Paralysis"), 100);
    _move.SetStatusCondition(new StatusCondition("Poison"), 95);
    _move.RemoveStatusCondition(new StatusCondition("Burn"));
    _move.Update(ActorId);
    await _moveRepository.SaveAsync(_move);

    ReplaceMovePayload payload = new("Tackle")
    {
      DisplayName = "  Tackle  ",
      Description = "    ",
      Accuracy = 100,
      Power = 40,
      PowerPoints = 35,
      Reference = "https://bulbapedia.bulbagarden.net/wiki/Tackle_(move)",
      Notes = "    "
    };
    payload.StatisticChanges.Add(new StatisticChange(PokemonStatistic.Defense, -1));
    payload.StatisticChanges.Add(new StatisticChange(PokemonStatistic.SpecialAttack, 1));
    payload.StatusConditions.Add(new InflictedStatusCondition("Freeze", 10));
    payload.StatusConditions.Add(new InflictedStatusCondition("Poison", 70));
    ReplaceMoveCommand command = new(_move.Id.ToGuid(), payload, version);
    Move? move = await Pipeline.ExecuteAsync(command);
    Assert.NotNull(move);

    Assert.Equal(_move.Id.ToGuid(), move.Id);
    Assert.Equal(_move.Version + 1, move.Version);
    Assert.Equal(Actor, move.CreatedBy);
    Assert.Equal(Actor, move.UpdatedBy);
    Assert.True(move.CreatedOn < move.UpdatedOn);

    Assert.Equal(payload.UniqueName, move.UniqueName);
    Assert.Equal(payload.DisplayName.CleanTrim(), move.DisplayName);
    Assert.Equal(_move.Description.Value, move.Description);
    Assert.Equal(payload.Accuracy, move.Accuracy);
    Assert.Equal(payload.Power, move.Power);
    Assert.Equal(payload.PowerPoints, move.PowerPoints);
    Assert.Equal(payload.Reference.CleanTrim(), move.Reference);
    Assert.Equal(payload.Notes.CleanTrim(), move.Notes);

    Assert.Equal(3, move.StatisticChanges.Count);
    Assert.Contains(move.StatisticChanges, x => x.Statistic == PokemonStatistic.Attack && x.Stages == -1);
    Assert.Contains(move.StatisticChanges, x => x.Statistic == PokemonStatistic.SpecialAttack && x.Stages == 1);
    Assert.Contains(move.StatisticChanges, x => x.Statistic == PokemonStatistic.Defense && x.Stages == -1);

    Assert.Equal(3, move.StatusConditions.Count);
    Assert.Contains(move.StatusConditions, x => x.StatusCondition == "Freeze" && x.Chance == 10);
    Assert.Contains(move.StatusConditions, x => x.StatusCondition == "Paralysis" && x.Chance == 100);
    Assert.Contains(move.StatusConditions, x => x.StatusCondition == "Poison" && x.Chance == 70);
  }

  [Fact(DisplayName = "It should return null when the move could not be found.")]
  public async Task It_should_return_null_when_the_move_could_not_be_found()
  {
    ReplaceMovePayload payload = new("Tackle");
    ReplaceMoveCommand command = new(Id: Guid.NewGuid(), payload, Version: null);
    Assert.Null(await Pipeline.ExecuteAsync(command));
  }

  [Fact(DisplayName = "It should throw UniqueNameAlreadyUsedException when the unique name is already used.")]
  public async Task It_should_throw_UniqueNameAlreadyUsedException_when_the_unique_name_is_already_used()
  {
    MoveAggregate move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueNameUnit(MoveAggregate.UniqueNameSettings, "Scratch"), ActorId);
    await _moveRepository.SaveAsync(move);

    ReplaceMovePayload payload = new(move.UniqueName.Value)
    {
      Accuracy = 100,
      Power = 40,
      PowerPoints = 35
    };
    ReplaceMoveCommand command = new(_move.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<MoveAggregate>>(async () => await Pipeline.ExecuteAsync(command));
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal(nameof(payload.UniqueName), exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_is_not_valid()
  {
    MoveAggregate move = new(PokemonType.Normal, MoveCategory.Status, new UniqueNameUnit(MoveAggregate.UniqueNameSettings, "Growl"), ActorId);
    await _moveRepository.SaveAsync(move);

    ReplaceMovePayload payload = new(move.UniqueName.Value)
    {
      Accuracy = 100,
      Power = 40,
      PowerPoints = 40
    };
    payload.StatisticChanges.Add(new StatisticChange(PokemonStatistic.Attack, -1));
    ReplaceMoveCommand command = new(move.Id.ToGuid(), payload, Version: null);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await Pipeline.ExecuteAsync(command));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal("NullValidator", error.ErrorCode);
    Assert.Equal("Power", error.PropertyName);
  }
}
