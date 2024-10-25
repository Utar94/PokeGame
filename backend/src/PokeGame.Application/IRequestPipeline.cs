using MediatR;

namespace PokeGame.Application;

public interface IRequestPipeline
{
  Task<T> ExecuteAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default);
}
