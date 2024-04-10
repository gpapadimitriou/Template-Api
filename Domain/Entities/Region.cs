using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; }
        public List<Municipality> Municipalities { get; set; } = new List<Municipality>();
    }
}
