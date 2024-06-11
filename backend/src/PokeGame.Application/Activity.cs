using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;

namespace PokeGame.Application;

public abstract record Activity : IActivity
{
  private ActivityContext? _context = null;
  protected ActivityContext Context => _context ?? throw new InvalidOperationException($"The activity has not been contextualized. You must call the '{nameof(Contextualize)}' method once.");

  public Actor Actor
  {
    get
    {
      if (Context.User != null)
      {
        return new Actor(Context.User);
      }
      else if (Context.ApiKey != null)
      {
        return new Actor(Context.ApiKey);
      }

      return Actor.System;
    }
  }
  public ActorId ActorId => new(Actor.Id);

  public void Contextualize(ActivityContext context)
  {
    if (_context != null)
    {
      throw new InvalidOperationException($"The activity has already been contextualized. You may only call the '{nameof(Contextualize)}' method once.");
    }

    _context = context;
  }
}
