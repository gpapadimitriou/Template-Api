using Application.User.Commands.Register;
using Application.User.Queries.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FarmaSenseApi.Controllers
{
    public class RoleController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
           Summary = "Get a list of user roles"
            )
        ]
        public async Task<ActionResult<List<RolesDto>>> GetRolesAsync() =>
           await Mediator.Send(new GetRolesQuery());
    }
}
