using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Web.Models.Api.Game
{
  public class GameInventoryModel
  {
    public GameInventoryModel(ListModel<InventoryModel> inventory, int money)
    {
      Money = money;

      Dictionary<ItemCategory, IEnumerable<GameInventoryLineModel>> items = inventory.Items.Where(x => x.Item != null)
        .GroupBy(x => x.Item!.Category)
        .ToDictionary(x => x.Key, x => x.Select(y => new GameInventoryLineModel(y)));

      if (items.TryGetValue(ItemCategory.Medicine, out IEnumerable<GameInventoryLineModel>? medicine))
      {
        Medicine = medicine;
      }
      if (items.TryGetValue(ItemCategory.PokeBall, out IEnumerable<GameInventoryLineModel>? pokeBalls))
      {
        PokeBalls = pokeBalls;
      }
      if (items.TryGetValue(ItemCategory.BattleItem, out IEnumerable<GameInventoryLineModel>? battleItems))
      {
        BattleItems = battleItems;
      }
      if (items.TryGetValue(ItemCategory.Berry, out IEnumerable<GameInventoryLineModel>? berries))
      {
        Berries = berries;
      }
      if (items.TryGetValue(ItemCategory.OtherItem, out IEnumerable<GameInventoryLineModel>? otherItems))
      {
        OtherItems = otherItems;
      }
      if (items.TryGetValue(ItemCategory.TM, out IEnumerable<GameInventoryLineModel>? tms))
      {
        TMs = tms;
      }
      if (items.TryGetValue(ItemCategory.Treasure, out IEnumerable<GameInventoryLineModel>? treasures))
      {
        Treasures = treasures;
      }
      if (items.TryGetValue(ItemCategory.KeyItem, out IEnumerable<GameInventoryLineModel>? keyItems))
      {
        KeyItems = keyItems;
      }
    }

    public IEnumerable<GameInventoryLineModel> Medicine { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> PokeBalls { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> BattleItems { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> Berries { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> OtherItems { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> TMs { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> Treasures { get; set; } = Enumerable.Empty<GameInventoryLineModel>();
    public IEnumerable<GameInventoryLineModel> KeyItems { get; set; } = Enumerable.Empty<GameInventoryLineModel>();

    public int Money { get; set; }
  }
}
