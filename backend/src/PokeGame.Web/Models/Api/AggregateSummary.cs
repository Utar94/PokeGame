using PokeGame.Application.Models;

namespace PokeGame.Web.Models.Api
{
  public abstract class AggregateSummary
  {
    protected AggregateSummary(AggregateModel model)
    {
      ArgumentNullException.ThrowIfNull(model);

      Id = model.Id;
      UpdatedAt = model.UpdatedAt ?? model.CreatedAt;
    }

    public Guid Id { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
