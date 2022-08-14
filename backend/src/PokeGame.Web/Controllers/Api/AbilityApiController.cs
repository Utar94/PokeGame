using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Core.Abilities;
using PokeGame.Core.Abilities.Models;
using PokeGame.Core.Abilities.Payloads;
using PokeGame.Core.Models;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/abilities")]
  public class AbilityApiController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IAbilityService _service;

    public AbilityApiController(IMapper mapper, IAbilityService service)
    {
      _mapper = mapper;
      _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<AbilityModel>> CreateAsync([FromBody] CreateAbilityPayload payload, CancellationToken cancellationToken)
    {
      AbilityModel ability = await _service.CreateAsync(payload, cancellationToken);
      var uri = new Uri($"/api/abilities/{ability.Id}", UriKind.Relative);

      return Created(uri, ability);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AbilityModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _service.DeleteAsync(id, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<AbilitySummary>> GetAsync(string? search,
      AbilitySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      ListModel<AbilityModel> abilities = await _service.GetAsync(search,
        sort, desc,
        index, count,
        cancellationToken);

      return Ok(new ListModel<AbilitySummary>(_mapper.Map<IEnumerable<AbilitySummary>>(abilities.Items), abilities.Total));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AbilityModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      AbilityModel? ability = await _service.GetAsync(id, cancellationToken);
      if (ability == null)
      {
        return NotFound();
      }

      return Ok(ability);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AbilityModel>> UpdateAsync(Guid id, [FromBody] UpdateAbilityPayload payload, CancellationToken cancellationToken)
    {
      return Ok(await _service.UpdateAsync(id, payload, cancellationToken));
    }
  }
}
