using Logitar;
using MongoDB.Driver;
using PokeGame.Application.Logging;
using PokeGame.Infrastructure;
using PokeGame.MongoDB.Entities;
using System.Text.Json;

namespace PokeGame.MongoDB.Repositories;

internal class LogRepository : ILogRepository
{
  private readonly IMongoCollection<LogEntity> _logs;
  private readonly JsonSerializerOptions _serializerOptions = new();

  public LogRepository(IMongoDatabase database, IServiceProvider serviceProvider)
  {
    _logs = database.GetCollection<LogEntity>("logs");
    _serializerOptions.Converters.AddRange(serviceProvider.GetJsonConverters());
  }

  public async Task SaveAsync(Log log, CancellationToken cancellationToken)
  {
    LogEntity entity = new(log, _serializerOptions);

    await _logs.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
  }
}
