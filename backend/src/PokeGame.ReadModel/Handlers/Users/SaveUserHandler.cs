using Logitar.Portal.Core.Users.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Users
{
  internal class SaveUserHandler : INotificationHandler<SaveUser>
  {
    private readonly ReadContext _readContext;

    public SaveUserHandler(ReadContext readContext)
    {
      _readContext = readContext;
    }

    public async Task Handle(SaveUser notification, CancellationToken cancellationToken)
    {
      UserModel user = notification.User;

      UserEntity? entity = await _readContext.Users
        .SingleOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

      if (entity != null && entity.Version >= user.Version)
      {
        return;
      }
      else if (entity == null)
      {
        entity = new UserEntity { Id = user.Id };
        _readContext.Users.Add(entity);
      }

      entity.Synchronize(user);

      await _readContext.SaveChangesAsync(cancellationToken);
    }
  }
}
