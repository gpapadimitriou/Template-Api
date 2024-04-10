using Application.Municipality.Queries.GetMunicipalities;
using Application.User.Queries.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FarmaSenseApi.Controllers
{
    public class MunicipalityController : ApiControllerBase
    {
        [Authorize(Roles = "Διαχειριστής")]
        [HttpGet]
        [SwaggerOperation(
           Summary = "Get a list of municipalities"
            )
        ]
        public async Task<ActionResult<List<MunicipalityRegionDto>>> GetMunicipalitiesAsync() =>
           await Mediator.Send(new GetMunicipalitiesQuery());
    }
}
