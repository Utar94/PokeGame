using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

internal static class PokemonDb
{
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();

  public static class Abilities
  {
    public static readonly TableId Table = new(nameof(PokemonContext.Abilities));

    public static readonly ColumnId AbilityId = new(nameof(AbilityEntity.AbilityId), Table);
    public static readonly ColumnId AggregateId = new(nameof(AbilityEntity.AggregateId), Table);
    public static readonly ColumnId CreatedBy = new(nameof(AbilityEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(AbilityEntity.CreatedOn), Table);
    public static readonly ColumnId Description = new(nameof(AbilityEntity.Description), Table);
    public static readonly ColumnId DisplayName = new(nameof(AbilityEntity.DisplayName), Table);
    public static readonly ColumnId Notes = new(nameof(AbilityEntity.Notes), Table);
    public static readonly ColumnId Reference = new(nameof(AbilityEntity.Reference), Table);
    public static readonly ColumnId UniqueName = new(nameof(AbilityEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(AbilityEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(AbilityEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(AbilityEntity.UpdatedOn), Table);
    public static readonly ColumnId Version = new(nameof(AbilityEntity.Version), Table);
  }

  public static class Actors
  {
    public static readonly TableId Table = new(nameof(PokemonContext.Actors));

    public static readonly ColumnId ActorId = new(nameof(ActorEntity.ActorId), Table);
    public static readonly ColumnId DisplayName = new(nameof(ActorEntity.DisplayName), Table);
    public static readonly ColumnId EmailAddress = new(nameof(ActorEntity.EmailAddress), Table);
    public static readonly ColumnId Id = new(nameof(ActorEntity.Id), Table);
    public static readonly ColumnId IsDeleted = new(nameof(ActorEntity.IsDeleted), Table);
    public static readonly ColumnId PictureUrl = new(nameof(ActorEntity.PictureUrl), Table);
    public static readonly ColumnId Type = new(nameof(ActorEntity.Type), Table);
  }

  public static class LogEvents
  {
    public static readonly TableId Table = new(nameof(PokemonContext.LogEvents));

    public static readonly ColumnId EventId = new(nameof(LogEventEntity.EventId));
    public static readonly ColumnId LogEventId = new(nameof(LogEventEntity.LogEventId));
    public static readonly ColumnId LogId = new(nameof(LogEventEntity.LogId));
  }

  public static class LogExceptions
  {
    public static readonly TableId Table = new(nameof(PokemonContext.LogExceptions));

    public static readonly ColumnId Data = new(nameof(LogExceptionEntity.Data));
    public static readonly ColumnId HResult = new(nameof(LogExceptionEntity.HResult));
    public static readonly ColumnId HelpLink = new(nameof(LogExceptionEntity.HelpLink));
    public static readonly ColumnId LogExceptionId = new(nameof(LogExceptionEntity.LogExceptionId));
    public static readonly ColumnId LogId = new(nameof(LogExceptionEntity.LogId));
    public static readonly ColumnId Message = new(nameof(LogExceptionEntity.Message));
    public static readonly ColumnId Source = new(nameof(LogExceptionEntity.Source));
    public static readonly ColumnId StackTrace = new(nameof(LogExceptionEntity.StackTrace));
    public static readonly ColumnId TargetSite = new(nameof(LogExceptionEntity.TargetSite));
    public static readonly ColumnId Type = new(nameof(LogExceptionEntity.Type));
  }

  public static class Logs
  {
    public static readonly TableId Table = new(nameof(PokemonContext.Logs));

    public static readonly ColumnId ActivityData = new(nameof(LogEntity.ActivityData));
    public static readonly ColumnId ActivityType = new(nameof(LogEntity.ActivityType));
    public static readonly ColumnId ActorId = new(nameof(LogEntity.ActorId));
    public static readonly ColumnId AdditionalInformation = new(nameof(LogEntity.AdditionalInformation));
    public static readonly ColumnId ApiKeyId = new(nameof(LogEntity.ApiKeyId));
    public static readonly ColumnId CorrelationId = new(nameof(LogEntity.CorrelationId));
    public static readonly ColumnId Destination = new(nameof(LogEntity.Destination));
    public static readonly ColumnId Duration = new(nameof(LogEntity.Duration));
    public static readonly ColumnId EndedOn = new(nameof(LogEntity.EndedOn));
    public static readonly ColumnId HasErrors = new(nameof(LogEntity.HasErrors));
    public static readonly ColumnId IsCompleted = new(nameof(LogEntity.IsCompleted));
    public static readonly ColumnId Level = new(nameof(LogEntity.Level));
    public static readonly ColumnId LogId = new(nameof(LogEntity.LogId));
    public static readonly ColumnId Method = new(nameof(LogEntity.Method));
    public static readonly ColumnId OperationName = new(nameof(LogEntity.OperationName));
    public static readonly ColumnId OperationType = new(nameof(LogEntity.OperationType));
    public static readonly ColumnId SessionId = new(nameof(LogEntity.SessionId));
    public static readonly ColumnId Source = new(nameof(LogEntity.Source));
    public static readonly ColumnId StartedOn = new(nameof(LogEntity.StartedOn));
    public static readonly ColumnId StatusCode = new(nameof(LogEntity.StatusCode));
    public static readonly ColumnId TenantId = new(nameof(LogEntity.TenantId));
    public static readonly ColumnId UniqueId = new(nameof(LogEntity.UniqueId));
    public static readonly ColumnId UserId = new(nameof(LogEntity.UserId));
  }

  public static class Moves
  {
    public static readonly TableId Table = new(nameof(PokemonContext.Moves));

    public static readonly ColumnId Accuracy = new(nameof(MoveEntity.AggregateId), Table);
    public static readonly ColumnId AggregateId = new(nameof(MoveEntity.AggregateId), Table);
    public static readonly ColumnId Category = new(nameof(MoveEntity.Category), Table);
    public static readonly ColumnId CreatedBy = new(nameof(MoveEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(MoveEntity.CreatedOn), Table);
    public static readonly ColumnId Description = new(nameof(MoveEntity.Description), Table);
    public static readonly ColumnId DisplayName = new(nameof(MoveEntity.DisplayName), Table);
    public static readonly ColumnId MoveId = new(nameof(MoveEntity.MoveId), Table);
    public static readonly ColumnId Notes = new(nameof(MoveEntity.Notes), Table);
    public static readonly ColumnId Power = new(nameof(MoveEntity.Power), Table);
    public static readonly ColumnId PowerPoints = new(nameof(MoveEntity.PowerPoints), Table);
    public static readonly ColumnId Reference = new(nameof(MoveEntity.Reference), Table);
    public static readonly ColumnId StatisticChanges = new(nameof(MoveEntity.StatisticChanges), Table);
    public static readonly ColumnId StatusConditions = new(nameof(MoveEntity.StatusConditions), Table);
    public static readonly ColumnId Type = new(nameof(MoveEntity.Type), Table);
    public static readonly ColumnId UniqueName = new(nameof(MoveEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(MoveEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(MoveEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(MoveEntity.UpdatedOn), Table);
    public static readonly ColumnId Version = new(nameof(MoveEntity.Version), Table);
  }

  public static class MoveCategories
  {
    public static readonly TableId Table = new(nameof(PokemonContext.MoveCategories));

    public static readonly ColumnId MoveCategoryId = new("MoveCategoryId", Table);
    public static readonly ColumnId Name = new("Name", Table);
  }

  public static class PokemonTypes
  {
    public static readonly TableId Table = new(nameof(PokemonContext.PokemonTypes));

    public static readonly ColumnId Name = new("Name", Table);
    public static readonly ColumnId PokemonTypeId = new("PokemonTypeId", Table);
  }
}
