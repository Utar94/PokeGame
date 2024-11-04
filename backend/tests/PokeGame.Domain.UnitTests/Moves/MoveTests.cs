using PokeGame.Contracts;
using PokeGame.Contracts.Moves;

namespace PokeGame.Domain.Moves;

[Trait(Traits.Category, Categories.Unit)]
public class MoveTests
{
  private static readonly UserId _userId = UserId.NewId();

  private readonly Move _move = new(PokemonType.Normal, MoveCategory.Physical, new Name("Tackle"), _userId);

  [Fact(DisplayName = "AddVolatileCondition: it should add a new volatile condition.")]
  public void AddVolatileCondition_it_should_add_a_new_volatile_condition()
  {
    _move.ClearChanges();

    VolatileCondition volatileCondition = new("Curse");
    _move.AddVolatileCondition(volatileCondition);
    Assert.Contains(volatileCondition, _move.VolatileConditions);

    _move.Update(_userId);
    Assert.True(_move.HasChanges);
    Assert.True(Assert.Single(_move.Changes) is Move.UpdatedEvent e && e.VolatileConditions[volatileCondition] == ActionKind.Add);
  }

  [Fact(DisplayName = "AddVolatileCondition: it should not do anything when the volatile condition already exists.")]
  public void AddVolatileCondition_it_should_not_do_anything_when_the_volatile_condition_already_exists()
  {
    VolatileCondition volatileCondition = new("Curse");
    _move.AddVolatileCondition(volatileCondition);
    _move.Update(_userId);
    _move.ClearChanges();

    _move.AddVolatileCondition(volatileCondition);
    _move.Update(_userId);
    Assert.False(_move.HasChanges);
    Assert.Empty(_move.Changes);
  }

  [Fact(DisplayName = "It should throw ArgumentOutOfRangeException when constructing with an undefined category.")]
  public void It_should_throw_ArgumentOutOfRangeException_when_constructing_with_an_undefined_category()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Move(PokemonType.Normal, (MoveCategory)(-1), new Name("Growl"), _userId));
    Assert.Equal("category", exception.ParamName);
  }

  [Fact(DisplayName = "It should throw ArgumentOutOfRangeException when constructing with an undefined type.")]
  public void It_should_throw_ArgumentOutOfRangeException_when_constructing_with_an_undefined_type()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Move((PokemonType)(-1), MoveCategory.Status, new Name("Growl"), _userId));
    Assert.Equal("type", exception.ParamName);
  }

  [Theory(DisplayName = "It should throw ArgumentOutOfRangeException when setting an invalid accuracy.")]
  [InlineData(-10)]
  [InlineData(0)]
  [InlineData(100 + 1)]
  public void It_should_throw_ArgumentOutOfRangeException_when_setting_an_invalid_accuracy(int accuracy)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.Accuracy = accuracy);
    Assert.Equal("Accuracy", exception.ParamName);
  }

  [Theory(DisplayName = "It should throw ArgumentOutOfRangeException when setting an invalid power.")]
  [InlineData(-100)]
  [InlineData(0)]
  [InlineData(Move.PowerMaximumValue + 1)]
  public void It_should_throw_ArgumentOutOfRangeException_when_setting_an_invalid_power(int power)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.Power = power);
    Assert.Equal("Power", exception.ParamName);
  }

  [Fact(DisplayName = "It should throw ArgumentOutOfRangeException when setting an undefined kind.")]
  public void It_should_throw_ArgumentOutOfRangeException_when_setting_an_undefined_kind()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.Kind = (MoveKind)(-1));
    Assert.Equal("Kind", exception.ParamName);
  }

  [Theory(DisplayName = "It should throw ArgumentOutOfRangeException when setting invalid power points.")]
  [InlineData(-20)]
  [InlineData(0)]
  [InlineData(Move.PowerPointsMaximumValue + 1)]
  public void It_should_throw_ArgumentOutOfRangeException_when_setting_invalid_power_points(int powerPoints)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.PowerPoints = powerPoints);
    Assert.Equal("PowerPoints", exception.ParamName);
  }

  [Fact(DisplayName = "It should throw StatusMoveCannotHavePowerException when setting power to a status move.")]
  public void It_should_throw_StatusMoveCannotHavePowerException_when_setting_power_to_a_status_move()
  {
    Move move = new(PokemonType.Normal, MoveCategory.Status, new Name("Growl"), _userId);
    int power = 100;
    var exception = Assert.Throws<StatusMoveCannotHavePowerException>(() => move.Power = power);
    Assert.Equal(move.Id.ToGuid(), exception.MoveId);
    Assert.Equal(power, exception.Power);
    Assert.Equal("Power", exception.PropertyName);
  }

  [Fact(DisplayName = "RemoveVolatileCondition: it should not do anything when the volatile condition does not exist.")]
  public void RemoveVolatileConditionn_it_should_not_do_anything_when_the_volatile_condition_does_not_exist()
  {
    _move.ClearChanges();

    VolatileCondition volatileCondition = new("Curse");
    _move.RemoveVolatileCondition(volatileCondition);
    _move.Update(_userId);

    Assert.False(_move.HasChanges);
    Assert.Empty(_move.Changes);
  }

  [Fact(DisplayName = "RemoveVolatileCondition: it should remove an existing volatile condition.")]
  public void RemoveVolatileCondition_it_should_remove_an_existing_volatile_condition()
  {
    VolatileCondition volatileCondition = new("Curse");
    _move.AddVolatileCondition(volatileCondition);
    _move.Update(_userId);
    _move.ClearChanges();

    _move.RemoveVolatileCondition(volatileCondition);
    Assert.Empty(_move.VolatileConditions);
    _move.Update(_userId);

    Assert.True(_move.HasChanges);
    Assert.True(Assert.Single(_move.Changes) is Move.UpdatedEvent e && e.VolatileConditions[volatileCondition] == ActionKind.Remove);
  }

  [Fact(DisplayName = "SetStatisticChange: it should add a new statistic change.")]
  public void SetStatisticChange_it_should_add_a_new_statistic_change()
  {
    _move.ClearChanges();

    _move.SetStatisticChange(BattleStatistic.Attack, stages: -1);
    Assert.Equal(-1, _move.StatisticChanges[BattleStatistic.Attack]);
    _move.Update(_userId);

    Assert.True(_move.HasChanges);
    Assert.True(Assert.Single(_move.Changes) is Move.UpdatedEvent e
      && e.StatisticChanges.Single().Key == BattleStatistic.Attack
      && e.StatisticChanges.Single().Value == -1);
  }

  [Fact(DisplayName = "SetStatisticChange: it should not do anything when removing a statistic change that does not exist.")]
  public void SetStatisticChange_it_should_not_do_anything_when_removing_a_statistic_change_that_does_not_exist()
  {
    _move.SetStatisticChange(BattleStatistic.Attack, stages: -1);
    _move.Update(_userId);
    _move.ClearChanges();

    _move.SetStatisticChange(BattleStatistic.Defense, stages: 0);
    Assert.DoesNotContain(BattleStatistic.Defense, _move.StatisticChanges.Keys);
    _move.Update(_userId);

    Assert.False(_move.HasChanges);
    Assert.Empty(_move.Changes);
  }

  [Fact(DisplayName = "SetStatisticChange: it should not do anything when updating a statistic that has no change.")]
  public void SetStatisticChange_it_should_not_do_anything_when_updating_a_statistic_that_has_no_change()
  {
    _move.SetStatisticChange(BattleStatistic.Attack, stages: -1);
    _move.Update(_userId);
    _move.ClearChanges();

    _move.SetStatisticChange(BattleStatistic.Attack, stages: -1);
    Assert.Equal(-1, _move.StatisticChanges[BattleStatistic.Attack]);
    _move.Update(_userId);

    Assert.False(_move.HasChanges);
    Assert.Empty(_move.Changes);
  }

  [Fact(DisplayName = "SetStatisticChange: it should remove an existing statistic change.")]
  public void SetStatisticChange_it_should_remove_an_existing_statistic_change()
  {
    _move.SetStatisticChange(BattleStatistic.Attack, stages: -1);
    _move.Update(_userId);
    _move.ClearChanges();

    _move.SetStatisticChange(BattleStatistic.Attack, stages: 0);
    Assert.Empty(_move.StatisticChanges);
    _move.Update(_userId);

    Assert.True(_move.HasChanges);
    Assert.True(Assert.Single(_move.Changes) is Move.UpdatedEvent e
      && e.StatisticChanges.Single().Key == BattleStatistic.Attack
      && e.StatisticChanges.Single().Value == 0);
  }

  [Fact(DisplayName = "SetStatisticChange: it should throw ArgumentOutOfRangeException when setting an undefined statistic.")]
  public void SetStatisticChange_it_should_throw_ArgumentOutOfRangeException_when_setting_an_undefined_statistic()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.SetStatisticChange((BattleStatistic)(-1), stages: 0));
    Assert.Equal("statistic", exception.ParamName);
  }

  [Theory(DisplayName = "SetStatisticChange: it should throw ArgumentOutOfRangeException when setting out-of-bound stages.")]
  [InlineData(Move.StageMinimumValue - 1)]
  [InlineData(Move.StageMaximumValue + 1)]
  public void SetStatisticChange_it_should_throw_ArgumentOutOfRangeException_when_setting_out_of_bound_stages(int stages)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _move.SetStatisticChange(BattleStatistic.Evasion, stages));
    Assert.Equal("stages", exception.ParamName);
  }

  [Fact(DisplayName = "SetStatisticChange: it should update an existing statistic change.")]
  public void SetStatisticChange_it_should_update_an_existing_statistic_change()
  {
    _move.SetStatisticChange(BattleStatistic.Attack, stages: -1);
    _move.Update(_userId);
    _move.ClearChanges();

    _move.SetStatisticChange(BattleStatistic.Attack, stages: -2);
    Assert.Equal(-2, _move.StatisticChanges[BattleStatistic.Attack]);
    _move.Update(_userId);

    Assert.True(_move.HasChanges);
    Assert.True(Assert.Single(_move.Changes) is Move.UpdatedEvent e
      && e.StatisticChanges.Single().Key == BattleStatistic.Attack
      && e.StatisticChanges.Single().Value == -2);
  }
}
