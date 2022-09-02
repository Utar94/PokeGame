namespace PokeGame.Core.Trainers.Events
{
  public class DeletedEvent : DeletedEventBase
  {
    public DeletedEvent(Guid userId) : base(userId)
    {
    }
  }
}
