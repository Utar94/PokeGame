using FluentValidation;
using PokeGame.Core.Inventories.Models;
using PokeGame.Core.Items;
using PokeGame.Core.Models;
using PokeGame.Core.Trainers;

namespace PokeGame.Core.Inventories
{
  internal class InventoryService : IInventoryService
  {
    private readonly IItemQuerier _itemQuerier;
    private readonly IMappingService _mappingService;
    private readonly IInventoryQuerier _querier;
    private readonly ITrainerQuerier _trainerQuerier;
    private readonly IRepository<Trainer> _trainerRepository;
    private readonly IValidator<Trainer> _trainerValidator;
    private readonly IUserContext _userContext;

    public InventoryService(
      IItemQuerier itemQuerier,
      IMappingService mappingService,
      IInventoryQuerier querier,
      ITrainerQuerier trainerQuerier,
      IRepository<Trainer> trainerRepository,
      IValidator<Trainer> trainerValidator,
      IUserContext userContext
    )
    {
      _itemQuerier = itemQuerier;
      _mappingService = mappingService;
      _querier = querier;
      _trainerQuerier = trainerQuerier;
      _trainerRepository = trainerRepository;
      _trainerValidator = trainerValidator;
      _userContext = userContext;
    }

    public async Task<InventoryModel> AddAsync(Guid trainerId, Guid itemId, ushort quantity, bool buy, CancellationToken cancellationToken)
    {
      Trainer trainer = await _trainerQuerier.GetWithInventoryAsync(trainerId, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);
      Item item = await _itemQuerier.GetAsync(itemId, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(itemId);

      if (buy)
      {
        trainer.BuyItem(item, quantity, _userContext.Id);
      }
      else
      {
        trainer.AddItem(item, quantity, _userContext.Id);
      }

      Inventory? inventory = trainer.Inventory.SingleOrDefault(x => x.Item?.Equals(item) == true);
      if (inventory == null)
      {
        inventory = new(trainer, item);
        trainer.Inventory.Add(inventory);
      }
      inventory.Quantity += quantity;

      _trainerValidator.ValidateAndThrow(trainer);

      await _trainerRepository.SaveAsync(trainer, cancellationToken);

      return await _mappingService.MapAsync<InventoryModel>(inventory, cancellationToken);
    }

    public async Task<InventoryModel> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken)
    {
      Trainer trainer = await _trainerQuerier.GetAsync(trainerId, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);
      Item item = await _itemQuerier.GetAsync(itemId, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(itemId);

      Inventory inventory = await _querier.GetAsync(trainer.Sid, item.Sid, readOnly: true, cancellationToken)
        ?? new(trainer, item);

      return await _mappingService.MapAsync<InventoryModel>(inventory, cancellationToken);
    }

    public async Task<ListModel<InventoryModel>> GetAsync(Guid trainerId, ItemCategory? category, string? search,
      InventorySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      Trainer trainer = await _trainerQuerier.GetAsync(trainerId, readOnly: true, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);

      PagedList<Inventory> inventory = await _querier.GetPagedAsync(trainer.Sid, category, search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Inventory, InventoryModel>(inventory, cancellationToken);
    }

    public async Task<InventoryModel> RemoveAsync(Guid trainerId, Guid itemId, ushort quantity, bool sell, CancellationToken cancellationToken)
    {
      Trainer trainer = await _trainerQuerier.GetWithInventoryAsync(trainerId, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);
      Item item = await _itemQuerier.GetAsync(itemId, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(itemId);

      if (sell)
      {
        trainer.SellItem(item, quantity, _userContext.Id);
      }
      else
      {
        trainer.RemoveItem(item, quantity, _userContext.Id);
      }

      Inventory inventory = trainer.Inventory.Single(x => x.Item?.Equals(item) == true);
      inventory.Quantity -= quantity;
      if (inventory.Quantity == 0)
      {
        trainer.Inventory.Remove(inventory);
      }

      _trainerValidator.ValidateAndThrow(trainer);

      await _trainerRepository.SaveAsync(trainer, cancellationToken);

      return await _mappingService.MapAsync<InventoryModel>(inventory, cancellationToken);
    }
  }
}
