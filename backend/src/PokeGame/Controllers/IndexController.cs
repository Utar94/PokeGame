using Microsoft.AspNetCore.Mvc;
using PokeGame.Models.Index;

namespace PokeGame.Controllers;

[ApiController]
[Route("")]
public class IndexController : ControllerBase
{
  [HttpGet]
  public ActionResult<ApiVersion> Get() => Ok(ApiVersion.Current);
}
