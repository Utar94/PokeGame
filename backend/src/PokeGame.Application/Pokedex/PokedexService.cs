using PokeGame.Application.Models;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokedex
{
  internal class PokedexService : IPokedexService
  {
    private readonly IPokedexQuerier _querier;
    private readonly IRepository _repository;

    public PokedexService(IPokedexQuerier querier, IRepository repository)
    {
      _querier = querier;
      _repository = repository;
    }

    public async Task DeleteAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(trainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);

      trainer.RemovePokedex(speciesId);

      await _repository.SaveAsync(trainer, cancellationToken);
    }

    public async Task<PokedexModel?> GetAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(trainerId, speciesId, cancellationToken);
    }

    public async Task<ListModel<PokedexModel>> GetAsync(Guid trainerId, bool? hasCaught, string? search, PokemonType? type,
      PokedexSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(trainerId, hasCaught, search, type,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<PokedexModel> SaveAsync(Guid trainerId, Guid speciesId, bool hasCaught, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(trainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);

      trainer.SavePokedex(speciesId, hasCaught);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainerId, speciesId, cancellationToken)
        ?? throw new PokedexNotFoundException(trainerId, speciesId);
    }
  }
}
