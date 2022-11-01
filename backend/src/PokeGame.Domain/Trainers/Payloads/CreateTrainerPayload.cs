namespace PokeGame.Domain.Trainers.Payloads
{
  public class CreateTrainerPayload : SaveTrainerPayload
  {
    public int Number { get; set; }

    public TrainerGender Gender { get; set; }
  }
}
