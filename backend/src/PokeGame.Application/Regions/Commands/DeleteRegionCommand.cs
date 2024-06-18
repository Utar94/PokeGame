using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Commands;

public record DeleteRegionCommand(Guid Id) : Activity, IRequest<Region?>;
