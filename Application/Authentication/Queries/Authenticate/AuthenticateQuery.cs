using Application.Common.Guards;
using Application.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Queries.Authenticate
{
    public class AuthenticateQuery : IRequest<AuthenticateDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, AuthenticateDto>
    {
        private readonly ITokenClaimsService _tokenClaimsService;
        public AuthenticateQueryHandler(ITokenClaimsService tokenClaimsService)
        {
            _tokenClaimsService = tokenClaimsService;
        }

        public async Task<AuthenticateDto> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            var result = await _tokenClaimsService.AreCredentialsValid(request.Username, request.Password);
            Guard.Against.InvalidCredentials(result);

            var authResponse = new AuthenticateDto();
            var tokenResponse = await _tokenClaimsService.GetTokenAsync(request.Username);
            authResponse.AccessToken = tokenResponse.AccessToken;
            authResponse.RefreshToken = tokenResponse.RefreshToken;
            return authResponse;
        }
    }
}
