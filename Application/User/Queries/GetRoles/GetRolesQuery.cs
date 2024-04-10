using Application.Authentication.Queries.Authenticate;
using Application.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Queries.Roles
{
    public class GetRolesQuery : IRequest<List<RolesDto>>
    {
    }

    public class RolesQueryHandler : IRequestHandler<GetRolesQuery, List<RolesDto>>
    {
        private readonly ITokenClaimsService _tokenClaimsService;
        public RolesQueryHandler(ITokenClaimsService tokenClaimsService)
        {
            _tokenClaimsService = tokenClaimsService;
        }

        public async Task<List<RolesDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var rolesDto = new List<RolesDto>();

            var roles = await _tokenClaimsService.GetAllRoles();
            foreach (var role in roles.Where(r => r == "Καλλιεργητής"))
            {
                rolesDto.Add(new RolesDto()
                {
                    Name = role
                });
            }
            return rolesDto;
        }
    }
}
