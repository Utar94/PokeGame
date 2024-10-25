using Bogus;
using FluentValidation.Results;
using Logitar.Portal.Contracts.Users;
using MediatR;
using Moq;
using PokeGame.Application.Accounts.Events;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Commands;

[Trait(Traits.Category, Categories.Unit)]
public class SaveProfileCommandHandlerTests
{
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();

  private readonly Mock<IPublisher> _publisher = new();
  private readonly Mock<IUserService> _userService = new();

  private readonly SaveProfileCommandHandler _handler;

  public SaveProfileCommandHandlerTests()
  {
    _handler = new(_publisher.Object, _userService.Object);
  }

  [Fact(DisplayName = "It should save the user profile.")]
  public async Task It_should_save_the_user_profile()
  {
    User user = new(_faker.Person.UserName);
    SaveProfilePayload payload = new(_faker.Person.FirstName, _faker.Person.LastName, "fr", "America/Montreal")
    {
      MultiFactorAuthenticationMode = MultiFactorAuthenticationMode.Phone,
      Birthdate = _faker.Person.DateOfBirth,
      Gender = _faker.Person.Gender.ToString()
    };
    _userService.Setup(x => x.SaveProfileAsync(user, payload, _cancellationToken)).ReturnsAsync(user);

    SaveProfileCommand command = new(payload);
    command.Contextualize(user);
    User result = await _handler.Handle(command, _cancellationToken);

    Assert.Same(result, user);

    _publisher.Verify(x => x.Publish(It.Is<UserUpdatedEvent>(y => y.User == user), _cancellationToken), Times.Once);
  }

  [Fact(DisplayName = "It should throw ValidationException when the payload is not valid.")]
  public async Task It_should_throw_ValidationException_when_the_payload_it_not_valid()
  {
    SaveProfilePayload payload = new(_faker.Person.FirstName, _faker.Person.LastName, "fr", "America/Montreal")
    {
      Birthdate = DateTime.Now.AddYears(1)
    };
    SaveProfileCommand command = new(payload);
    var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _handler.Handle(command, _cancellationToken));
    ValidationFailure error = Assert.Single(exception.Errors);
    Assert.Equal("PastValidator", error.ErrorCode);
    Assert.Equal("Birthdate.Value", error.PropertyName);
  }
}
