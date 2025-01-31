using Moq;
using PokeGame.Application.Speciez.Models;

namespace PokeGame.Application.Speciez.Queries;

[Trait(Traits.Category, Categories.Unit)]
public class ReadSpeciesQueryHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<ISpeciesQuerier> _speciesQuerier = new();

  private readonly ReadSpeciesQueryHandler _handler;

  private readonly SpeciesModel _kanto = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Pikachu"
  };
  private readonly SpeciesModel _johto = new()
  {
    Id = Guid.NewGuid(),
    UniqueName = "Raichu"
  };

  public ReadSpeciesQueryHandlerTests()
  {
    _handler = new(_speciesQuerier.Object);

    _speciesQuerier.Setup(x => x.ReadAsync(_kanto.Id, _cancellationToken)).ReturnsAsync(_kanto);
    _speciesQuerier.Setup(x => x.ReadAsync(_kanto.UniqueName, _cancellationToken)).ReturnsAsync(_kanto);
    _speciesQuerier.Setup(x => x.ReadAsync(_johto.Id, _cancellationToken)).ReturnsAsync(_johto);
    _speciesQuerier.Setup(x => x.ReadAsync(_johto.UniqueName, _cancellationToken)).ReturnsAsync(_johto);
  }

  [Fact(DisplayName = "It should return null when no species was found.")]
  public async Task Given_NoneFound_When_Handle_Then_NullReturned()
  {
    ReadSpeciesQuery query = new(Id: null, UniqueName: null);
    SpeciesModel? species = await _handler.Handle(query, _cancellationToken);
    Assert.Null(species);
  }

  [Fact(DisplayName = "It should return the species found by ID.")]
  public async Task Given_FoundById_When_Handle_Then_Returned()
  {
    ReadSpeciesQuery query = new(_kanto.Id, _kanto.UniqueName);
    SpeciesModel? species = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(species);
    Assert.Same(_kanto, species);
  }

  [Fact(DisplayName = "It should return the species found by unique name.")]
  public async Task Given_FoundByUniqueName_When_Handle_Then_Returned()
  {
    ReadSpeciesQuery query = new(Guid.Empty, _kanto.UniqueName);
    SpeciesModel? species = await _handler.Handle(query, _cancellationToken);
    Assert.NotNull(species);
    Assert.Same(_kanto, species);
  }

  [Fact(DisplayName = "It should throw TooManyResultsException when more than one species were found.")]
  public async Task Given_ManyFound_When_Handle_Then_TooManyResultsException()
  {
    ReadSpeciesQuery query = new(_kanto.Id, _johto.UniqueName);
    var exception = await Assert.ThrowsAsync<TooManyResultsException<SpeciesModel>>(async () => await _handler.Handle(query, _cancellationToken));
    Assert.Equal(1, exception.ExpectedCount);
    Assert.Equal(2, exception.ActualCount);
  }
}
