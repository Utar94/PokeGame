﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Moves.Events;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Moves
{
  internal class MoveDeletedHandler : INotificationHandler<MoveDeleted>
  {
    private readonly ReadContext _readContext;

    public MoveDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(MoveDeleted notification, CancellationToken cancellationToken)
    {
      MoveEntity? move = await _readContext.Moves
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (move != null)
      {
        _readContext.Moves.Remove(move);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
