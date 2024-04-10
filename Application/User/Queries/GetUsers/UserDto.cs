using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Queries.Get
{
    public class UserDto : IMapFrom<Domain.Entities.User>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string DateCreated { get; set; }
        public string LastLogin { get; set; }
        public string Role { get; set; }
        public string Parent { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.User, UserDto>()
              .ForMember(d => d.Parent, opt => opt.Ignore())
              .ForMember(d => d.DateCreated, opt => opt.MapFrom(s => ((DateTimeOffset)s.DateCreated).ToUnixTimeMilliseconds()))
              .ForMember(d => d.LastLogin, opt => opt.MapFrom(s => ((DateTimeOffset)s.LastLogin).ToUnixTimeMilliseconds()));
        }

    }
}
