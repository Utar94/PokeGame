﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Web.Models.Api.Locale;
using System.Globalization;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.AuthenticatedUser)]
  [Route("api/locales")]
  public class LocaleApiController : ControllerBase
  {
    private static readonly IEnumerable<LocaleSummary> _locales = CultureInfo.GetCultures(CultureTypes.AllCultures)
      .Where(x => x.LCID != 4096 && !string.IsNullOrEmpty(x.Name))
      .Select(culture => new LocaleSummary(culture));

    [HttpGet]
    public ActionResult<IEnumerable<LocaleSummary>> Get() => Ok(_locales);
  }
}
