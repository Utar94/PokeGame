using MediatR;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

internal record SaveRegionCommand(RegionAggregate Region) : IRequest;
