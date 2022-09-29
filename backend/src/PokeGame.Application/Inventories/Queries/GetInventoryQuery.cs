using MediatR;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Inventories.Queries
{
  public class GetInventoryQuery : IRequest<ListModel<InventoryModel>>
  {
    public ItemCategory? Category { get; set; }
    public string? Search { get; set; }
    public Guid TrainerId { get; set; }

    public InventorySort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
