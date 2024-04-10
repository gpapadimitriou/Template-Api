using Application.Authentication.Queries.Authenticate;
using Application.Common.Models;
using Application.User.Commands.Register;
using Application.User.Queries.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FarmaSenseApi.Controllers
{
    public class UserController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation(
           Summary = "Register a user"
            )
        ]
        public async Task<ActionResult<int>> RegisterAsync(RegisterCommand request) =>
           await Mediator.Send(request);

        [Authorize(Roles = "Διαχειριστής")]
        [HttpGet]
        [SwaggerOperation(
           Summary = "Get a list of users",
            Description ="From query filter parameters"
            )
        ]
        public async Task<ActionResult<PaginatedList<UserDto>>> GetUsersAsync([FromQuery] GetUsersQuery query) =>
          await Mediator.Send(query);
    }
}
