using Application.Authentication.Queries.Authenticate;
using Application.Common.Guards;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands.Register
{
    public class RegisterCommand : IRequest<int>
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IAsyncEfRepository<Domain.Entities.User> _userRepository;
        public RegisterCommandHandler(ITokenClaimsService tokenClaimsService, IAsyncEfRepository<Domain.Entities.User> userRepository)
        {
            _tokenClaimsService = tokenClaimsService;
            _userRepository = userRepository;
        }

        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.EmailIsTaken(await _tokenClaimsService.GetApplicationUser(request.Email));
            Guard.Against.InvalidRole(await _tokenClaimsService.GetAllRoles(), request.Role);
            var user = await _userRepository.AddAsync(new Domain.Entities.User()
            {
                ApplicationUserId = await _tokenClaimsService.CreateIdentityUser(request.Email, request.Password, request.Role),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                UserName = request.Email,
                Role = request.Role,
            });

            return user.Id;
        }
    }
}
