using FluentValidation;
using MediatR;
using PokeGame.Application.Inventories.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Inventories.Mutations
{
  internal class AddInventoryMutationHandler : IRequestHandler<AddInventoryMutation, InventoryModel>
  {
    private readonly IInventoryQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Trainer> _trainerValidator;

    public AddInventoryMutationHandler(
      IInventoryQuerier querier,
      IRepository repository,
      IValidator<Trainer> trainerValidator
    )
    {
      _querier = querier;
      _repository = repository;
      _trainerValidator = trainerValidator;
    }

    public async Task<InventoryModel> Handle(AddInventoryMutation request, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(request.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.TrainerId);
      Item item = await _repository.LoadAsync<Item>(request.ItemId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(request.ItemId);

      if (request.Buy)
      {
        trainer.BuyItem(item, request.Quantity);
      }
      else
      {
        trainer.AddItem(item, request.Quantity);
      }
      _trainerValidator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, item.Id, cancellationToken)
        ?? throw new InventoryNotFoundException(trainer.Id, item.Id);
    }
  }
}
