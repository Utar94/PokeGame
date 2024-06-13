using Microsoft.AspNetCore.Authorization;

namespace PokeGame.Authorization;

internal record RoleAuthorizationRequirement(string Role) : IAuthorizationRequirement;
