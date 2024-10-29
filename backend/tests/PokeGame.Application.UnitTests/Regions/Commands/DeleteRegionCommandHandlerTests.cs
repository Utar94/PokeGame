using Moq;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class DeleteRegionCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly Mock<IRegionQuerier> _regionQuerier = new();
  private readonly Mock<IRegionRepository> _regionRepository = new();

  private readonly DeleteRegionCommandHandler _handler;

  private readonly UserId _userId = UserId.NewId();
  private readonly Region _region;

  public DeleteRegionCommandHandlerTests()
  {
    _handler = new(_regionQuerier.Object, _regionRepository.Object);

    _region = new(new Name("Kanto"), _userId);
    _regionRepository.Setup(x => x.LoadAsync(_region.Id, _cancellationToken)).ReturnsAsync(_region);
  }

  [Fact(DisplayName = "It should delete an existing region.")]
  public async Task It_should_delete_an_existing_region()
  {
    DeleteRegionCommand command = new(_region.Id.ToGuid());
    command.Contextualize();

    RegionModel model = new();
    _regionQuerier.Setup(x => x.ReadAsync(_region, _cancellationToken)).ReturnsAsync(model);

    RegionModel? result = await _handler.Handle(command, _cancellationToken);
    Assert.NotNull(result);
    Assert.Same(model, result);

    _regionRepository.Verify(x => x.SaveAsync(It.Is<Region>(y => y.Equals(_region) && y.IsDeleted), _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should return null when the region could not be found.")]
  public async Task It_should_return_null_when_the_region_could_not_be_found()
  {
    DeleteRegionCommand command = new(Guid.NewGuid());
    command.Contextualize();

    Assert.Null(await _handler.Handle(command, _cancellationToken));
  }
}
