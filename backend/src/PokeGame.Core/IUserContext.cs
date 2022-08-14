namespace PokeGame.Core
{
  public interface IUserContext
  {
    Guid Id { get; }
    Guid SessionId { get; }
  }
}
