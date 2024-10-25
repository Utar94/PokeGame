using Logitar.EventSourcing;

namespace PokeGame.Application;

public abstract record Activity : IActivity
{
  [JsonIgnore]
  private ActivityContext? _context = null;
  [JsonIgnore]
  protected ActivityContext Context => _context ?? throw new InvalidOperationException($"The activity has not been contextualized. You must call the '{nameof(Contextualize)}' method once.");

  public virtual IActivity Anonymize()
  {
    return this;
  }

  public void Contextualize(ActivityContext context)
  {
    if (_context != null)
    {
      throw new InvalidOperationException($"The activity has already been contextualized. You may only call the '{nameof(Contextualize)}' method once.");
    }

    _context = context;
  }

  public ActorId GetActorId() => Context.User == null ? new() : new(Context.User.Id);
}
