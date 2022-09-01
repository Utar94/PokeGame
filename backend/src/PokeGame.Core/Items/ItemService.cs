using FluentValidation;
using PokeGame.Core.Items.Models;
using PokeGame.Core.Items.Payloads;
using PokeGame.Core.Models;

namespace PokeGame.Core.Items
{
  internal class ItemService : IItemService
  {
    private readonly IMappingService _mappingService;
    private readonly IItemQuerier _querier;
    private readonly IRepository<Item> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Item> _validator;

    public ItemService(
      IMappingService mappingService,
      IItemQuerier querier,
      IRepository<Item> repository,
      IUserContext userContext,
      IValidator<Item> validator
    )
    {
      _mappingService = mappingService;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<ItemModel> CreateAsync(CreateItemPayload payload, CancellationToken cancellationToken)
    {
      var item = new Item(payload, _userContext.Id);
      _validator.ValidateAndThrow(item);

      await _repository.SaveAsync(item, cancellationToken);

      return await _mappingService.MapAsync<ItemModel>(item, cancellationToken);
    }

    public async Task<ItemModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Item item = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(id);

      item.Delete(_userContext.Id);

      await _repository.SaveAsync(item, cancellationToken);

      return await _mappingService.MapAsync<ItemModel>(item, cancellationToken);
    }

    public async Task<ItemModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Item? item = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (item == null)
      {
        return null;
      }

      return await _mappingService.MapAsync<ItemModel>(item, cancellationToken);
    }

    public async Task<ListModel<ItemModel>> GetAsync(Category? category, string? search,
      ItemSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Item> items = await _querier.GetPagedAsync(category, search,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Item, ItemModel>(items, cancellationToken);
    }

    public async Task<ItemModel> UpdateAsync(Guid id, UpdateItemPayload payload, CancellationToken cancellationToken)
    {
      Item item = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(id);

      item.Update(payload, _userContext.Id);
      _validator.ValidateAndThrow(item);

      await _repository.SaveAsync(item, cancellationToken);

      return await _mappingService.MapAsync<ItemModel>(item, cancellationToken);
    }
  }
}
