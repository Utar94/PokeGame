namespace PokeGame.Infrastructure
{
  public interface IUserContext
  {
    Guid Id { get; }
    Guid SessionId { get; }
  }
}
