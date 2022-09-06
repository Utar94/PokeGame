using FluentValidation;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Payloads;

namespace PokeGame.Application.Items
{
  internal class ItemService : IItemService
  {
    private readonly IItemQuerier _querier;
    private readonly IRepository<Item> _repository;
    private readonly IValidator<Item> _validator;

    public ItemService(
      IItemQuerier querier,
      IRepository<Item> repository,
      IValidator<Item> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<ItemModel> CreateAsync(CreateItemPayload payload, CancellationToken cancellationToken)
    {
      var item = new Item(payload);
      _validator.ValidateAndThrow(item);

      await _repository.SaveAsync(item, cancellationToken);

      return await _querier.GetAsync(item.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(item.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Item item = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(id);

      item.Delete();

      await _repository.SaveAsync(item, cancellationToken);
    }

    public async Task<ItemModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(id, cancellationToken);
    }

    public async Task<ListModel<ItemModel>> GetAsync(ItemCategory? category, string? search,
      ItemSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(category, search,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<ItemModel> UpdateAsync(Guid id, UpdateItemPayload payload, CancellationToken cancellationToken)
    {
      Item item = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(id);

      item.Update(payload);
      _validator.ValidateAndThrow(item);

      await _repository.SaveAsync(item, cancellationToken);

      return await _querier.GetAsync(item.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(item.Id);
    }
  }
}
