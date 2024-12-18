using Logitar.EventSourcing;
using PokeGame.Domain.Regions;
using PokeGame.Domain.Speciez.Events;

namespace PokeGame.Domain.Speciez;

[Trait(Traits.Category, Categories.Unit)]
public class SpeciesTests
{
  private readonly UserId _userId = new(ActorId.NewId());

  private readonly Region _region;
  private readonly Species _species;

  public SpeciesTests()
  {
    _region = new(new UniqueName("Kanto"), _userId);
    _species = new(number: 25, SpeciesCategory.Standard, new UniqueName("Pikachu"), GrowthRate.MediumFast, new Friendship(70), new CatchRate(190), _userId);
  }

  [Fact(DisplayName = "ctor: it should throw ArgumentOutOfRangeException when the category is not defined.")]
  public void Given_CategoryIsNotDefined_When_ctor_Then_ArgumentOutOfRangeException()
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Species(number: 25, (SpeciesCategory)(-1), new UniqueName("Pikachu"), GrowthRate.MediumFast, new Friendship(70), new CatchRate(190), _userId));
    Assert.Equal("category", exception.ParamName);
  }

  [Theory(DisplayName = "ctor: it should throw ArgumentOutOfRangeException when the number is negative or 0.")]
  [InlineData(-5)]
  [InlineData(0)]
  public void Given_NumberIsNegativeOrZero_When_ctor_Then_ArgumentOutOfRangeException(int number)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Species(number, SpeciesCategory.Standard, new UniqueName("Pikachu"), GrowthRate.MediumFast, new Friendship(70), new CatchRate(190), _userId));
    Assert.Equal("number", exception.ParamName);
  }

  [Theory(DisplayName = "SetRegionalNumber: it should not do anything when there is no change.")]
  [InlineData(null)]
  [InlineData(25)]
  public void Given_NoChange_When_SetRegionalNumber_Then_DoNothing(int? number)
  {
    if (number.HasValue)
    {
      _species.SetRegionalNumber(_region, number.Value, _userId);
    }
    _species.ClearChanges();

    _species.SetRegionalNumber(_region, number, _userId);

    Assert.False(_species.HasChanges);
    Assert.Empty(_species.Changes);
  }

  [Theory(DisplayName = "SetRegionalNumber: it should raise the correct event when the number changed.")]
  [InlineData(null)]
  [InlineData(25)]
  public void Given_NumberChanged_When_SetRegionalNumber_Then_RaiseEvent(int? number)
  {
    if (!number.HasValue)
    {
      _species.SetRegionalNumber(_region, number: 25, _userId);
    }

    _species.SetRegionalNumber(_region, number, _userId);

    if (number.HasValue)
    {
      Assert.Equal(number, _species.RegionalNumbers[_region.Id]);
    }
    else
    {
      Assert.False(_species.RegionalNumbers.ContainsKey(_region.Id));
    }
    Assert.Contains(_species.Changes, change => change is SpeciesRegionalNumberChanged changed && changed.RegionId == _region.Id && changed.Number == number);
  }

  [Theory(DisplayName = "SetRegionalNumber: it should throw ArgumentOutOfRangeException when the number is negative or 0.")]
  [InlineData(-8)]
  [InlineData(0)]
  public void Given_NumberIsNegativeOrZero_When_SetRegionalNumber_Then_ArgumentOutOfRangeException(int number)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _species.SetRegionalNumber(_region, number, _userId));
    Assert.Equal("number", exception.ParamName);
    Assert.StartsWith("The number must be greater than 0.", exception.Message);
  }
}
