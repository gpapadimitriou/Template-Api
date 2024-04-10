using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Specifications.Municipality
{
    public class MunicipalityWithRegionSpecification : Specification<Domain.Entities.Municipality>
    {
        public MunicipalityWithRegionSpecification()
        {
            Query
                .Include(x => x.Region);
        }
    }
}
