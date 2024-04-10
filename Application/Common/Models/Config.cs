using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class Config
    {
        public class JwtConfiguration
        {
            public string Secret { get; set; }
            public TimeSpan TokenLifetime { get; set; }
        }
    }
}
