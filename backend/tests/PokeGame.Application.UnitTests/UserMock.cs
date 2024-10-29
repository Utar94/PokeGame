using Bogus;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.Application;

internal class UserMock : User
{
  public UserMock(Faker? faker = null)
  {
    faker ??= new();

    DateTime now = DateTime.Now;
    Realm = new RealmMock();

    Id = Guid.NewGuid();
    Version = 1;
    CreatedOn = now;
    UpdatedOn = now;

    UniqueName = faker.Person.UserName;

    Email = new Email(faker.Person.Email)
    {
      IsVerified = true,
      VerifiedOn = now
    };
    IsConfirmed = true;

    FirstName = faker.Person.FirstName;
    LastName = faker.Person.LastName;
    FullName = faker.Person.FullName;

    Birthdate = faker.Person.DateOfBirth;
    Gender = faker.Person.Gender.ToString().ToLowerInvariant();
    Locale = new Locale("fr-CA");
    TimeZone = "America/Montreal";

    Picture = faker.Person.Avatar;

    AuthenticatedOn = now;

    Roles.Add(new Role("admin")
    {
      Id = Guid.NewGuid(),
      DisplayName = "Administrator",
      Realm = Realm
    });

    Actor actor = new(this);
    CreatedBy = actor;
    UpdatedBy = actor;
    Email.VerifiedBy = actor;
  }
}
