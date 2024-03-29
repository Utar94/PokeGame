﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Domain.Trainers.Events;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class TrainerDeletedHandler : INotificationHandler<TrainerDeleted>
  {
    private readonly ReadContext _readContext;

    public TrainerDeletedHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(TrainerDeleted notification, CancellationToken cancellationToken)
    {
      TrainerEntity? trainer = await _readContext.Trainers
        .SingleOrDefaultAsync(x => x.Id == notification.AggregateId, cancellationToken);

      if (trainer != null)
      {
        _readContext.Trainers.Remove(trainer);
        await _readContext.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
