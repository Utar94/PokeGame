using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Commands;

public record CreateRegionCommand(CreateRegionPayload Payload) : Activity, IRequest<Region>;
