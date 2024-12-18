using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Speciez.Events;

public record SpeciesUpdated : DomainEvent, INotification
{
  public UniqueName? UniqueName { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }

  public GrowthRate? GrowthRate { get; set; }
  public Friendship? BaseFriendship { get; set; }
  public CatchRate? CatchRate { get; set; }

  public Change<Url>? Link { get; set; }
  public Change<Notes>? Notes { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueName != null || DisplayName != null
    || GrowthRate != null || BaseFriendship != null || CatchRate != null
    || Link != null || Notes != null;
}
