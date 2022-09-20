using FluentValidation;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Inventories
{
  internal class InventoryService : IInventoryService
  {
    private readonly IItemQuerier _itemQuerier;
    private readonly IInventoryQuerier _querier;
    private readonly IRepository _repository;
    private readonly ITrainerQuerier _trainerQuerier;
    private readonly IValidator<Trainer> _trainerValidator;

    public InventoryService(
      IItemQuerier itemQuerier,
      IInventoryQuerier querier,
      IRepository repository,
      ITrainerQuerier trainerQuerier,
      IValidator<Trainer> trainerValidator
    )
    {
      _itemQuerier = itemQuerier;
      _querier = querier;
      _repository = repository;
      _trainerQuerier = trainerQuerier;
      _trainerValidator = trainerValidator;
    }

    public async Task<InventoryModel> AddAsync(Guid trainerId, Guid itemId, ushort quantity, bool buy, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(trainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);
      Item item = await _repository.LoadAsync<Item>(itemId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(itemId);

      if (buy)
      {
        trainer.BuyItem(item, quantity);
      }
      else
      {
        trainer.AddItem(item, quantity);
      }
      _trainerValidator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, item.Id, cancellationToken)
        ?? throw new InventoryNotFoundException(trainer.Id, item.Id);
    }

    public async Task<InventoryModel> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken)
    {
      TrainerModel trainer = await _trainerQuerier.GetAsync(trainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);
      ItemModel item = await _itemQuerier.GetAsync(itemId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(itemId);

      InventoryModel? model = await _querier.GetAsync(trainer.Id, item.Id, cancellationToken);
      if (model == null)
      {
        return new InventoryModel
        {
          Item = item,
          Quantity = 0
        };
      }

      return model;
    }

    public async Task<ListModel<InventoryModel>> GetAsync(Guid trainerId, ItemCategory? category, string? search,
      InventorySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      TrainerModel trainer = await _trainerQuerier.GetAsync(trainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);

      return await _querier.GetPagedAsync(trainer.Id, category, search,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<InventoryModel> RemoveAsync(Guid trainerId, Guid itemId, ushort quantity, bool sell, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(trainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);
      Item item = await _repository.LoadAsync<Item>(itemId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(itemId);

      if (sell)
      {
        trainer.SellItem(item, quantity);
      }
      else
      {
        trainer.RemoveItem(item, quantity);
      }
      _trainerValidator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      InventoryModel? model = await _querier.GetAsync(trainer.Id, item.Id, cancellationToken);
      if (model == null)
      {
        ItemModel itemModel = await _itemQuerier.GetAsync(item.Id, cancellationToken)
          ?? throw new EntityNotFoundException<Item>(item.Id);

        return new InventoryModel
        {
          Item = itemModel,
          Quantity = 0
        };
      }

      return model;
    }
  }
}
