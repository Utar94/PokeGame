using AutoMapper;
using PokeGame.Core.Models;

namespace PokeGame.Core
{
  internal class MappingService : IMappingService
  {
    private readonly IMapper _mapper;

    public MappingService(IMapper mapper)
    {
      _mapper = mapper;
    }

    public Task<T> MapAsync<T>(object source, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(source);

      return Task.FromResult(_mapper.Map<T>(source));
    }

    public Task<ListModel<TDestination>> MapAsync<TSource, TDestination>(PagedList<TSource> list, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(list);

      return Task.FromResult(new ListModel<TDestination>(_mapper.Map<IEnumerable<TDestination>>(list), list.Total));
    }
  }
}
