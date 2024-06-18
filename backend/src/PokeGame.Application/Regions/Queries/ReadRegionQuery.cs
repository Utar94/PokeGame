using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

public record ReadRegionQuery(Guid? Id, string? UniqueName) : Activity, IRequest<Region?>;
