using Logitar.EventSourcing;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.Domain.Regions;

[Trait(Traits.Category, Categories.Unit)]
public class RegionTests
{
  private readonly RegionId _regionId = RegionId.NewId();
  private readonly UserId _userId = new(Guid.NewGuid());
  private readonly Region _region;

  public RegionTests()
  {
    _region = new Region(new UniqueName("hisui"), _userId, _regionId);
  }

  [Fact(DisplayName = "Delete: it should mark the region as deleted.")]
  public void Delete_it_should_mark_the_region_as_deleted()
  {
    _region.Delete(_userId);
    Assert.True(_region.IsDeleted);
  }

  [Fact(DisplayName = "Delete: it should not do anything when the region is deleted.")]
  public void Delete_it_should_not_do_anything_when_the_region_is_deleted()
  {
    _region.Delete(_userId);
    Assert.True(_region.IsDeleted);

    _region.ClearChanges();
    Assert.Empty(_region.Changes);
    Assert.False(_region.HasChanges);

    _region.Delete(_userId);
    Assert.Empty(_region.Changes);
    Assert.False(_region.HasChanges);
  }

  [Fact(DisplayName = "It should construct the correct region.")]
  public void It_should_construct_the_correct_region()
  {
    Assert.Equal("hisui", _region.UniqueName.Value);
    Assert.Equal(_userId.ActorId, _region.CreatedBy);
    Assert.Equal(_regionId, _region.Id);
  }

  [Fact(DisplayName = "Update: it should not do anything when there is no change.")]
  public void Update_it_should_not_do_anything_when_there_is_no_change()
  {
    _region.ClearChanges();
    Assert.Empty(_region.Changes);
    Assert.False(_region.HasChanges);

    RegionUpdates updates = new()
    {
      UniqueName = _region.UniqueName
    };
    _region.Update(updates, _userId);
    Assert.Empty(_region.Changes);
    Assert.False(_region.HasChanges);
  }

  [Fact(DisplayName = "Update: it should update the region.")]
  public void Update_it_should_update_the_region()
  {
    RegionUpdates updates = new()
    {
      DisplayName = new Change<DisplayName>(new DisplayName("Kanto")),
      Description = new Change<Description>(new Description("The Kanto region is a region of the Pokémon world.")),
      Link = new Change<Url>(new Url("https://bulbapedia.bulbagarden.net/wiki/Kanto")),
      Notes = new Change<Notes>(new Notes("Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh."))
    };
    _region.Update(updates, _userId);

    Assert.Equal(updates.DisplayName.Value, _region.DisplayName);
    Assert.Equal(updates.Description.Value, _region.Description);
    Assert.Equal(updates.Link.Value, _region.Link);
    Assert.Equal(updates.Notes.Value, _region.Notes);

    DomainEvent @event = Assert.Single(_region.Changes, change => change is RegionUpdated);
    Assert.True(@event is RegionUpdated updated && updated.UniqueName == null);
  }
}
