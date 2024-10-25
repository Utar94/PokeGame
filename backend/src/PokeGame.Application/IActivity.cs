namespace PokeGame.Application;

public interface IActivity
{
  IActivity Anonymize();
  void Contextualize(ActivityContext context);
}
