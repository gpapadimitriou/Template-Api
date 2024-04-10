using Application.Common.Mappings;
using Application.User.Queries.Get;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Municipality.Queries.GetMunicipalities
{
    public class MunicipalityDto : IMapFrom<Domain.Entities.Municipality>
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Municipality, MunicipalityDto>();
        }

    }
}
