using FluentValidation;
using PokeGame.Application.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Moves.Payloads;

namespace PokeGame.Application.Moves
{
  internal class MoveService : IMoveService
  {
    private readonly IMoveQuerier _querier;
    private readonly IRepository<Move> _repository;
    private readonly IValidator<Move> _validator;

    public MoveService(
      IMoveQuerier querier,
      IRepository<Move> repository,
      IValidator<Move> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<MoveModel> CreateAsync(CreateMovePayload payload, CancellationToken cancellationToken)
    {
      var move = new Move(payload);
      _validator.ValidateAndThrow(move);

      await _repository.SaveAsync(move, cancellationToken);

      return await _querier.GetAsync(move.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(move.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Move move = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(id);

      move.Delete();

      await _repository.SaveAsync(move, cancellationToken);
    }

    public async Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(id, cancellationToken);
    }

    public async Task<ListModel<MoveModel>> GetAsync(string? search, PokemonType? type,
      MoveSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(search, type,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<MoveModel> UpdateAsync(Guid id, UpdateMovePayload payload, CancellationToken cancellationToken)
    {
      Move move = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(id);

      move.Update(payload);
      _validator.ValidateAndThrow(move);

      await _repository.SaveAsync(move, cancellationToken);

      return await _querier.GetAsync(move.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Move>(move.Id);
    }
  }
}
