namespace PokeGame.Domain.Trainers.Payloads
{
  public class CreateTrainerPayload : SaveTrainerPayload
  {
    public Region Region { get; set; }
    public int Number { get; set; }

    public TrainerGender Gender { get; set; }
  }
}
