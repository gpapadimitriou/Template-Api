using Application.Interfaces;
using Application.Specifications.Municipality;
using Application.User.Queries.Roles;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Municipality.Queries.GetMunicipalities
{
    public class GetMunicipalitiesQuery : IRequest<List<MunicipalityRegionDto>>
    {
    }

    public class GetMunicipalitiesQueryHandler : IRequestHandler<GetMunicipalitiesQuery, List<MunicipalityRegionDto>>
    {
        private readonly IAsyncEfRepository<Domain.Entities.Municipality> _municipalityRepo;
        private readonly IMapper _mapper;
        public GetMunicipalitiesQueryHandler(IAsyncEfRepository<Domain.Entities.Municipality> municipalityRepo, IMapper mapper)
        {
            _municipalityRepo = municipalityRepo;
            _mapper = mapper;
        }

        public async Task<List<MunicipalityRegionDto>> Handle(GetMunicipalitiesQuery request, CancellationToken cancellationToken)
        {
            var municipalities = await _municipalityRepo.ListAsync(new MunicipalityWithRegionSpecification());

            var groupedMunicipalities = municipalities
                .GroupBy(m => m.RegionId)
                .Select(group => new MunicipalityRegionDto
                {
                    Region = group.First().Region.Name,
                    Municipalities = group.Select(m => _mapper.Map<MunicipalityDto>(m)).ToList()
                })
                .ToList();

            return groupedMunicipalities;
        }
    }
}
