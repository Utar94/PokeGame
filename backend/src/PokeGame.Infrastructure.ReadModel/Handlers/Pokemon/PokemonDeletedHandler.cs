using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Pokemon.Events;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class PokemonDeletedHandler : INotificationHandler<PokemonDeleted>
  {
    private readonly ReadContext _readContext;

    public PokemonDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(PokemonDeleted notification, CancellationToken cancellationToken)
    {
      PokemonEntity? move = await _readContext.Pokemon
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (move != null)
      {
        _readContext.Pokemon.Remove(move);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
