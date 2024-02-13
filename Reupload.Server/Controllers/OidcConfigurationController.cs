using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace Reupload.Server.Controllers;

public class OidcConfigurationController : Controller
{
    public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

    public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider)
    {
        ClientRequestParametersProvider = clientRequestParametersProvider;
    }

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        IDictionary<string, string> parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);

        return Ok(parameters);
    }
}