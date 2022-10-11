using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class MoveProfile : Profile
  {
    public MoveProfile()
    {
      CreateMap<MoveEntity, MoveModel>()
        .IncludeBase<Entity, AggregateModel>()
        .ForMember(x => x.StatisticStages, x => x.MapFrom(GetStatisticStages))
        .ForMember(x => x.VolatileConditions, x => x.MapFrom(GetVolatileConditions));
    }

    private static IEnumerable<StatisticStageModel> GetStatisticStages(MoveEntity move, MoveModel model)
    {
      string[] pairs = move.StatisticStages?.Split('|') ?? Array.Empty<string>();

      return pairs.Select(pair =>
      {
        string[] values = pair.Split(':');

        return new StatisticStageModel
        {
          Statistic = Enum.Parse<Statistic>(values[0]),
          Value = short.Parse(values[1])
        };
      });
    }

    private static IEnumerable<string> GetVolatileConditions(MoveEntity move, MoveModel model)
    {
      return move.VolatileConditions?.Split('|') ?? Array.Empty<string>();
    }
  }
}
