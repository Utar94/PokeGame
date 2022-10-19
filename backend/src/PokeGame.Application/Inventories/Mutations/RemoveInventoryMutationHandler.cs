using FluentValidation;
using MediatR;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Inventories.Mutations
{
  internal class RemoveInventoryMutationHandler : IRequestHandler<RemoveInventoryMutation, InventoryModel>
  {
    private readonly IItemQuerier _itemQuerier;
    private readonly IInventoryQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Trainer> _trainerValidator;

    public RemoveInventoryMutationHandler(
      IItemQuerier itemQuerier,
      IInventoryQuerier querier,
      IRepository repository,
      IValidator<Trainer> trainerValidator
    )
    {
      _itemQuerier = itemQuerier;
      _querier = querier;
      _repository = repository;
      _trainerValidator = trainerValidator;
    }

    public async Task<InventoryModel> Handle(RemoveInventoryMutation request, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(request.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.TrainerId);
      Item item = await _repository.LoadAsync<Item>(request.ItemId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(request.ItemId);

      if (request.Sell)
      {
        trainer.SellItem(item, request.Quantity);
      }
      else
      {
        trainer.RemoveItem(item, request.Quantity);
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
