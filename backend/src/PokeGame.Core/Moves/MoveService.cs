using FluentValidation;
using PokeGame.Core.Models;
using PokeGame.Core.Moves.Models;
using PokeGame.Core.Moves.Payloads;

namespace PokeGame.Core.Moves
{
  internal class MoveService : IMoveService
  {
    private readonly IMappingService _mappingService;
    private readonly IMoveQuerier _querier;
    private readonly IRepository<Move> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Move> _validator;

    public MoveService(
      IMappingService mappingService,
      IMoveQuerier querier,
      IRepository<Move> repository,
      IUserContext userContext,
      IValidator<Move> validator
    )
    {
      _mappingService = mappingService;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<MoveModel> CreateAsync(CreateMovePayload payload, CancellationToken cancellationToken)
    {
      var move = new Move(payload, _userContext.Id);
      _validator.ValidateAndThrow(move);

      await _repository.SaveAsync(move, cancellationToken);

      return await _mappingService.MapAsync<MoveModel>(move, cancellationToken);
    }

    public async Task<MoveModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Move move = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(id);

      move.Delete(_userContext.Id);

      await _repository.SaveAsync(move, cancellationToken);

      return await _mappingService.MapAsync<MoveModel>(move, cancellationToken);
    }

    public async Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Move? move = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (move == null)
      {
        return null;
      }

      return await _mappingService.MapAsync<MoveModel>(move, cancellationToken);
    }

    public async Task<ListModel<MoveModel>> GetAsync(string? search, PokemonType? type,
      MoveSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Move> moves = await _querier.GetPagedAsync(search, type,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Move, MoveModel>(moves, cancellationToken);
    }

    public async Task<MoveModel> UpdateAsync(Guid id, UpdateMovePayload payload, CancellationToken cancellationToken)
    {
      Move move = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(id);

      move.Update(payload, _userContext.Id);
      _validator.ValidateAndThrow(move);

      await _repository.SaveAsync(move, cancellationToken);

      return await _mappingService.MapAsync<MoveModel>(move, cancellationToken);
    }
  }
}
