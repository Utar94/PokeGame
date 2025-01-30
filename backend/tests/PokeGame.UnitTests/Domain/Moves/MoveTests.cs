using Logitar.EventSourcing;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Domain.Moves;

[Trait(Traits.Category, Categories.Unit)]
public class MoveTests
{
  private readonly ActorId _actorId = ActorId.NewId();

  private readonly Move _move;

  public MoveTests()
  {
    _move = new(PokemonType.Normal, MoveCategory.Physical, new UniqueName("Facade"), new PowerPoints(20), _actorId);
  }

  [Fact(DisplayName = "ctor: it should throw ArgumentOutOfRangeException when the category is not defined.")]
  public void Given_CategoryNotDefined_When_ctor_Then_ArgumentOutOfRangeException()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Move(PokemonType.Normal, (MoveCategory)(-1), new UniqueName("Facade"), new PowerPoints(20), _actorId));
    Assert.Equal("category", exception.ParamName);
  }

  [Fact(DisplayName = "ctor: it should throw ArgumentOutOfRangeException when the type is not defined.")]
  public void Given_TypeNotDefined_When_ctor_Then_ArgumentOutOfRangeException()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Move((PokemonType)(-1), MoveCategory.Physical, new UniqueName("Facade"), new PowerPoints(20), _actorId));
    Assert.Equal("type", exception.ParamName);
  }

  [Fact(DisplayName = "SetPower: it should throw StatusMoveCannotHavePowerException when the move is a Status move.")]
  public void Given_StatusMove_When_setPower_Then_StatusMoveCannotHavePowerException()
  {
    Move move = new(PokemonType.Normal, MoveCategory.Status, new UniqueName("Copycat"), new PowerPoints(20), _actorId);
    var exception = Assert.Throws<StatusMoveCannotHavePowerException>(() => move.Power = new Power(100));
    Assert.Equal(move.Id.ToGuid(), exception.MoveId);
  }

  [Theory(DisplayName = "SetStatisticChange: it should not do anything when there are no changes.")]
  [InlineData(-2)]
  [InlineData(0)]
  [InlineData(3)]
  public void Given_NoChanges_When_SetStatisticChange_Then_DoNothing(int stages)
  {
    PokemonStatistic statistic = PokemonStatistic.Evasion;

    if (stages != 0)
    {
      _move.SetStatisticChange(statistic, stages);
      _move.Update(_actorId);
    }
    _move.ClearChanges();

    _move.SetStatisticChange(statistic, stages);
    _move.Update(_actorId);

    Assert.False(_move.HasChanges);
    Assert.Empty(_move.Changes);
  }

  [Theory(DisplayName = "SetStatisticChange: it should set the statistic change when there are changes.")]
  [InlineData(-1)]
  [InlineData(0)]
  [InlineData(1)]
  public void Given_StatisticChange_When_SetStatisticChange_Then_ChangesApplied(int stages)
  {
    PokemonStatistic statistic = PokemonStatistic.Accuracy;

    if (stages == 0)
    {
      _move.SetStatisticChange(statistic, stages: 1);
      _move.Update(_actorId);
    }

    _move.SetStatisticChange(statistic, stages);
    _move.Update(_actorId);

    if (stages == 0)
    {
      Assert.False(_move.StatisticChanges.ContainsKey(statistic));
    }
    else
    {
      Assert.Equal(stages, _move.StatisticChanges[statistic]);
    }

    Assert.Contains(_move.Changes, change => change is MoveUpdated updated && updated.StatisticChanges[statistic] == stages);
  }

  [Fact(DisplayName = "SetStatisticChange: it should throw ArgumentException when the statistic is HP.")]
  public void Given_StatisticIsHP_When_SetStatisticChange_Then_ArgumentException()
  {
    var exception = Assert.Throws<ArgumentException>(() => _move.SetStatisticChange(PokemonStatistic.HP, stages: 0));
    Assert.Equal("statistic", exception.ParamName);
    Assert.StartsWith("The statistic cannot be HP.", exception.Message);
  }

  [Theory(DisplayName = "SetStatisticChange: it should throw ArgumentOutOfRangeException when the stage change is not in range.")]
  [InlineData(-10)]
  [InlineData(7)]
  public void Given_StagesNotInRange_When_SetStatisticChange_Then_ArgumentOutOfRangeException(int stages)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.SetStatisticChange(PokemonStatistic.Speed, stages));
    Assert.Equal("stages", exception.ParamName);
    Assert.StartsWith("The stages must range between -6 and 6.", exception.Message);
  }

  [Fact(DisplayName = "SetStatisticChange: it should throw ArgumentOutOfRangeException when the statistic is not defined.")]
  public void Given_StatisticNotDefined_When_SetStatisticChange_Then_ArgumentOutOfRangeException()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.SetStatisticChange((PokemonStatistic)(-1), stages: 0));
    Assert.Equal("statistic", exception.ParamName);
  }

  [Fact(DisplayName = "SetVolatileConditions: it should not do anything when there are no changes.")]
  public void Given_NoChanges_When_SetVolatileConditions_Then_DoNothing()
  {
    _move.SetVolatileConditions([new VolatileCondition("Mimic"), new VolatileCondition("Curse")]);
    _move.Update(_actorId);
    _move.ClearChanges();

    _move.SetVolatileConditions([new VolatileCondition("Mimic"), new VolatileCondition("Mimic"), new VolatileCondition("Curse")]);
    _move.Update(_actorId);

    Assert.False(_move.HasChanges);
    Assert.Empty(_move.Changes);
  }

  [Fact(DisplayName = "SetVolatileConditions: it should set the volatile conditions when there are changes.")]
  public void Given_VolatileConditionChanges_When_SetVolatileConditions_Then_ChangesApplied()
  {
    VolatileCondition[] volatileConditions = [new VolatileCondition("Mimic"), new VolatileCondition("Curse")];
    _move.SetVolatileConditions(volatileConditions);
    _move.Update(_actorId);

    Assert.True(volatileConditions.SequenceEqual(_move.VolatileConditions));
    Assert.Contains(_move.Changes, change => change is MoveUpdated updated && updated.VolatileConditions != null && updated.VolatileConditions.SequenceEqual(volatileConditions));
  }
}
