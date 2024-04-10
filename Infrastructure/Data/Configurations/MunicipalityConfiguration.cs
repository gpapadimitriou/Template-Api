using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class MunicipalityConfiguration : BaseEntityConfigurations<Municipality>
    {
        public override void Configure(EntityTypeBuilder<Municipality> builder)
        {
            base.Configure(builder);
        }
    }
}
