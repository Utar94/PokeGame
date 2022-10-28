using Logitar.Portal.Client;
using Logitar.Portal.Core.Users.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Users
{
  internal class SaveUsersHandler : INotificationHandler<SaveUsers>
  {
    private readonly ReadContext _readContext;
    private readonly IUserService _userService;

    public SaveUsersHandler(ReadContext readContext, IUserService userService)
    {
      _readContext = readContext;
      _userService = userService;
    }

    public async Task Handle(SaveUsers notification, CancellationToken cancellationToken)
    {
      Dictionary<Guid, UserEntity> entities = await _readContext.Users
        .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

      foreach (UserSummary summary in notification.Users.Items)
      {
        if (!entities.TryGetValue(summary.Id, out UserEntity? entity))
        {
          entity = new UserEntity { Id = summary.Id };
          _readContext.Users.Add(entity);
        }

        if (summary.UpdatedAt != (entity.UpdatedOn ?? entity.CreatedOn)
          || summary.UpdatedBy?.Id != (entity.UpdatedById ?? entity.CreatedById))
        {
          UserModel user = await _userService.GetAsync(summary.Id, cancellationToken);
          entity.Synchronize(user);
        }
      }

      HashSet<Guid> existingIds = notification.Users.Items.Select(x => x.Id).ToHashSet();
      foreach (UserEntity entity in entities.Values)
      {
        if (!existingIds.Contains(entity.Id))
        {
          _readContext.Users.Remove(entity);
        }
      }

      await _readContext.SaveChangesAsync(cancellationToken);
    }
  }
}
