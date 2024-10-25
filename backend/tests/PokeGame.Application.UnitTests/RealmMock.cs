using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Realms;

namespace PokeGame.Application;

internal class RealmMock : Realm
{
  public RealmMock() : base("pokegame", "G_`'^?byUB#.L&52AWY<,4sgQF8/)kqT")
  {
    Actor actor = Actor.System;
    DateTime now = DateTime.UtcNow;

    Id = Guid.NewGuid();
    Version = 2;
    CreatedBy = actor;
    CreatedOn = now;
    UpdatedBy = actor;
    UpdatedOn = now;

    DisplayName = "PokéGame";
    Description = "This is the realm of the Pokémon game management Web application.";

    DefaultLocale = new Locale("en");
    Url = "https://pokegame.francispion.ca";
  }
}
