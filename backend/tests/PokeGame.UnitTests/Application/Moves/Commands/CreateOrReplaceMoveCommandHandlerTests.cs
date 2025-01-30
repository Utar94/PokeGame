using Bogus;
using Logitar.EventSourcing;
using Moq;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class CreateOrReplaceMoveCommandHandlerTests
{
  private readonly ActorId _actorId = ActorId.NewId();
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IApplicationContext> _applicationContext = new();
  private readonly Mock<IMoveManager> _moveManager = new();
  private readonly Mock<IMoveQuerier> _moveQuerier = new();
  private readonly Mock<IMoveRepository> _moveRepository = new();

  private readonly CreateOrReplaceMoveCommandHandler _handler;

  public CreateOrReplaceMoveCommandHandlerTests()
  {
    _handler = new(_applicationContext.Object, _moveManager.Object, _moveQuerier.Object, _moveRepository.Object);

    _applicationContext.Setup(x => x.GetActorId()).Returns(_actorId);
  }

  [Theory(DisplayName = "It should create a new move.")]
  [InlineData(null)]
  [InlineData("978ae794-87dd-4aa1-9a93-95f0bb7c4c74")]
  public async Task Given_New_When_Handle_Then_Created(string? idValue)
  {
    Guid? id = idValue == null ? null : Guid.Parse(idValue);

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(It.IsAny<Move>(), _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceMovePayload payload = new()
    {
      Type = PokemonType.Ghost,
      Category = MoveCategory.Status,
      UniqueName = "Curse",
      DisplayName = " Curse ",
      Description = "  A move that has different effects depending on whether the user is a Ghost type or not.  ",
      PowerPoints = 10,
      InflictedStatus = new InflictedStatusModel(StatusCondition.Paralysis, chance: 10),
      StatisticChanges = [new StatisticChangeModel(PokemonStatistic.Speed, stages: -1)],
      VolatileConditions = ["Curse"],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Curse_(move)",
      Notes = "    ",
    };
    CreateOrReplaceMoveCommand command = new(id, payload, Version: null);
    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.True(result.Created);
    Assert.NotNull(result.Move);
    Assert.Same(model, result.Move);

    _moveManager.Verify(x => x.SaveAsync(
      It.Is<Move>(y => (!id.HasValue || y.Id.ToGuid() == id.Value) && y.CreatedBy == _actorId && y.UpdatedBy == _actorId
        && y.Type == payload.Type && y.Category == payload.Category
        && Comparisons.AreEqual(payload.UniqueName, y.UniqueName)
        && Comparisons.AreEqual(payload.DisplayName, y.DisplayName)
        && Comparisons.AreEqual(payload.Description, y.Description)
        && Comparisons.AreEqual(payload.Accuracy, y.Accuracy)
        && Comparisons.AreEqual(payload.Power, y.Power)
        && Comparisons.AreEqual(payload.PowerPoints, y.PowerPoints)
        && Comparisons.AreEqual(payload.InflictedStatus, y.InflictedStatus)
        && y.StatisticChanges.Count == 1 && y.StatisticChanges[PokemonStatistic.Speed] == -1
        && y.VolatileConditions.Single().Value == "Curse"
        && Comparisons.AreEqual(payload.Link, y.Link)
        && Comparisons.AreEqual(payload.Notes, y.Notes)),
      _cancellationToken), Times.Once());
  }

  [Fact(DisplayName = "It should replace an existing move.")]
  public async Task Given_Exists_When_Handle_Then_Replaced()
  {
    Move move = new(PokemonType.Ghost, MoveCategory.Status, new UniqueName("Curse"), new PowerPoints(15))
    {
      InflictedStatus = new InflictedStatus(StatusCondition.Paralysis, chance: 10)
    };
    move.SetStatisticChange(PokemonStatistic.Speed, stages: -1);
    move.SetVolatileConditions([new VolatileCondition("Curse")]);
    move.Update();
    _moveRepository.Setup(x => x.LoadAsync(move.Id, _cancellationToken)).ReturnsAsync(move);

    MoveModel model = new();
    _moveQuerier.Setup(x => x.ReadAsync(move, _cancellationToken)).ReturnsAsync(model);

    CreateOrReplaceMovePayload payload = new()
    {
      Type = PokemonType.Normal,
      Category = MoveCategory.Special,
      UniqueName = "ConfuseRay",
      DisplayName = " Confuse Ray  ",
      Description = "  The target is exposed to a sinister ray that causes confusion.  ",
      Accuracy = 100,
      PowerPoints = 10,
      VolatileConditions = ["Confusion"],
      Link = "https://bulbapedia.bulbagarden.net/wiki/Confuse_Ray_(move)",
      Notes = "    "
    };
    CreateOrReplaceMoveCommand command = new(move.Id.ToGuid(), payload, Version: null);
    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.False(result.Created);
    Assert.NotNull(result.Move);
    Assert.Same(model, result.Move);

    _moveManager.Verify(x => x.SaveAsync(move, _cancellationToken), Times.Once());

    Assert.Equal(_actorId, move.UpdatedBy);
    Assert.Equal(PokemonType.Ghost, move.Type);
    Assert.Equal(MoveCategory.Status, move.Category);
    Assertions.Equal(payload.UniqueName, move.UniqueName);
    Assertions.Equal(payload.DisplayName.Trim(), move.DisplayName);
    Assertions.Equal(payload.Description, move.Description);
    Assertions.Equal(payload.Accuracy, move.Accuracy);
    Assertions.Equal(payload.Power, move.Power);
    Assertions.Equal(payload.PowerPoints, move.PowerPoints);
    Assertions.Equal(payload.InflictedStatus, move.InflictedStatus);
    Assert.Empty(move.StatisticChanges);
    Assert.Equal(payload.VolatileConditions.Single(), Assert.Single(move.VolatileConditions).Value);
    Assertions.Equal(payload.Link, move.Link);
    Assertions.Equal(payload.Notes, move.Notes);
  }

  [Fact(DisplayName = "It should return null when the move was not found.")]
  public async Task Given_NotFound_When_Handle_Then_NullReturned()
  {
    CreateOrReplaceMovePayload payload = new()
    {
      UniqueName = "Facade",
      PowerPoints = 20
    };
    CreateOrReplaceMoveCommand command = new(Guid.NewGuid(), payload, Version: -1);
    CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
    Assert.Null(result.Move);
    Assert.False(result.Created);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task Given_InvalidPayload_When_Handle_Then_ValidationException()
  {
    CreateOrReplaceMovePayload payload = new()
    {
      Type = (PokemonType)(-1),
      Category = (MoveCategory)(-1),
      UniqueName = "Leer!",
      DisplayName = _faker.Random.String(999, minChar: 'A', maxChar: 'Z'),
      Accuracy = 0,
      Power = 1000,
      PowerPoints = 100,
      InflictedStatus = new InflictedStatusModel((StatusCondition)(-1), chance: 0),
      StatisticChanges = [new StatisticChangeModel(PokemonStatistic.HP, stages: 10)],
      VolatileConditions = [string.Empty, _faker.Random.String(999, minChar: 'A', maxChar: 'Z')],
      Link = "leer"
    };
    CreateOrReplaceMoveCommand command = new(Id: null, payload, Version: null);

    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));

    Assert.Equal(14, exception.Errors.Count());
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Type");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "Category");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "AllowedCharactersValidator" && e.PropertyName == "UniqueName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "DisplayName");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "Accuracy.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "Power.Value");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "PowerPoints");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "EnumValidator" && e.PropertyName == "InflictedStatus.Condition");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "InflictedStatus.Chance");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEqualValidator" && e.PropertyName == "StatisticChanges[0].Statistic");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "InclusiveBetweenValidator" && e.PropertyName == "StatisticChanges[0].Stages");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "VolatileConditions[0]");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "VolatileConditions[1]");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "UrlValidator" && e.PropertyName == "Link");
  }

  //[Fact(DisplayName = "It should update an existing move.")]
  //public async Task Given_Exists_When_Handle_Then_Updated()
  //{
  //  Move move = new(new UniqueName("Kanto"));
  //  _moveRepository.Setup(x => x.LoadAsync(move.Id, _cancellationToken)).ReturnsAsync(move);

  //  Move reference = new(move.UniqueName, move.CreatedBy, move.Id);
  //  _moveRepository.Setup(x => x.LoadAsync(reference.Id, reference.Version, _cancellationToken)).ReturnsAsync(reference);

  //  DisplayName displayName = new("Johto");
  //  move.DisplayName = displayName;
  //  move.Update();

  //  MoveModel model = new();
  //  _moveQuerier.Setup(x => x.ReadAsync(move, _cancellationToken)).ReturnsAsync(model);

  //  CreateOrReplaceMovePayload payload = new()
  //  {
  //    UniqueName = "Johto",
  //    DisplayName = "    ",
  //    Description = "    ",
  //    Link = "https://bulbapedia.bulbagarden.net/wiki/Johto",
  //    Notes = "  The Johto move (Japanese: ジョウト地方 Johto move) is a move of the Pokémon world. Johto is located west of Kanto, which together form a joint landmass that is south of Sinnoh and Sinjoh Ruins.  "
  //  };
  //  CreateOrReplaceMoveCommand command = new(move.Id.ToGuid(), payload, reference.Version);
  //  CreateOrReplaceMoveResult result = await _handler.Handle(command, _cancellationToken);
  //  Assert.False(result.Created);
  //  Assert.NotNull(result.Move);
  //  Assert.Same(model, result.Move);

  //  _moveManager.Verify(x => x.SaveAsync(move, _cancellationToken), Times.Once());

  //  Assert.Equal(_actorId, move.UpdatedBy);
  //  Assertions.Equal(payload.UniqueName, move.UniqueName);
  //  Assert.Equal(displayName, move.DisplayName);
  //  Assertions.Equal(payload.Description, move.Description);
  //  Assertions.Equal(payload.Link, move.Link);
  //  Assertions.Equal(payload.Notes, move.Notes);
  //} // TODO(fpion): implement
}
