using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VulnerableAPI.Database.Enums;

namespace VulnerableAPI.Controllers;

[ApiController]
[Route("v2/supported-currencies")]
public class SupportedCurrenciesController : ControllerBase
{

    [AllowAnonymous]
    [HttpGet]
    public ActionResult GetCurrencies()
    {
        return Ok(Enum.GetNames(typeof(Currency)).ToList());
    }
}
