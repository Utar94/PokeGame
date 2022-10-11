using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Species.Events;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Species
{
  internal class SpeciesDeletedHandler : INotificationHandler<SpeciesDeleted>
  {
    private readonly ReadContext _readContext;

    public SpeciesDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(SpeciesDeleted notification, CancellationToken cancellationToken)
    {
      SpeciesEntity? species = await _readContext.Species
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (species != null)
      {
        _readContext.Species.Remove(species);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
