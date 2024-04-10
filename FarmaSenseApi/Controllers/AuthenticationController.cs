using Application.User.Commands.Register;
using Application.Authentication.Queries.Authenticate;
using Application.Common.Models;
using Application.User.Queries.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FarmaSenseApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(
           Summary = "Authenticates a user",
           Description = "Username : test@gmail.com, Password : admin"
           )
       ]
        public async Task<ActionResult<AuthenticateDto>> AuthenticateAsync(AuthenticateQuery request) =>
           await Mediator.Send(request);
    }
}
