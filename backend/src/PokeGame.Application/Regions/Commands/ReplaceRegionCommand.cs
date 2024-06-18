using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Commands;

public record ReplaceRegionCommand(Guid Id, ReplaceRegionPayload Payload, long? Version) : Activity, IRequest<Region?>;
