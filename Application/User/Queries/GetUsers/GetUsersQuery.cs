using Application.Common.Models;
using Application.Interfaces;
using Application.Specifications.User;
using Ardalis.Specification;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Queries.Get
{
    public class GetUsersQuery : PaginatedQuery, IRequest<PaginatedList<UserDto>>
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public long DateCreatedFrom { get; set; }
        public long DateCreatedUntil { get; set; }

        public long LastLoginFrom { get; set; }
        public long LastLoginUntil { get; set; }
        public bool? IsPaginated { get; set; }
    }
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
    {
        private readonly IAsyncEfRepository<Domain.Entities.User> _userRepository;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IAsyncEfRepository<Domain.Entities.User> userRepository,
            ITokenClaimsService tokenClaimsService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenClaimsService = tokenClaimsService;
            _mapper = mapper;
        }
        public async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            bool isPaginated = false;
            if (request.IsPaginated is null)
                isPaginated = true;

            var users = await _userRepository.ListAsync(new UserFilterSpecification(request.Email, request.FirstName, request.LastName, request.Phone, request.Role, request.DateCreatedFrom, request.DateCreatedUntil, request.LastLoginFrom, request.LastLoginUntil, request.Page, request.Items, isPaginated));
            var totalNumberOfUsers = await _userRepository.CountAsync(new UserFilterSpecification(request.Email, request.FirstName, request.LastName, request.Phone, request.Role, request.DateCreatedFrom, request.DateCreatedUntil, request.LastLoginFrom, request.LastLoginUntil, request.Page, request.Items, false));

            return new PaginatedList<UserDto>(users.OrderByDescending(e => e.DateCreated).Select(_mapper.Map<UserDto>).ToList(), totalNumberOfUsers, request.Page, request.Items);

        }
    }
}
