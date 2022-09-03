using AutoMapper;
using PokeGame.Core.Items.Models;
using PokeGame.Core.Models;

namespace PokeGame.Core.Items
{
  internal class ItemProfile : Profile
  {
    public ItemProfile()
    {
      CreateMap<Item, ItemModel>()
        .IncludeBase<Aggregate, AggregateModel>();
      CreateMap<Item, ItemSummary>()
        .IncludeBase<Aggregate, AggregateSummary>();
      CreateMap<ItemModel, ItemSummary>()
        .IncludeBase<AggregateModel, AggregateSummary>();
    }
  }
}
