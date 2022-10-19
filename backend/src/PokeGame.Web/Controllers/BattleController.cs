using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokeGame.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("battle")]
  public class BattleController : Controller
  {
    [HttpGet]
    public ActionResult Index()
    {
      return View();
    }
  }
}
