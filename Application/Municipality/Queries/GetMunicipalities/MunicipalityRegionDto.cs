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
    public class MunicipalityRegionDto
    {
        public string Region { get; set; }
        public List<MunicipalityDto> Municipalities { get; set; }
    }
}
