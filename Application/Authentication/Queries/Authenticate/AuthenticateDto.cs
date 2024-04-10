using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Queries.Authenticate
{
    public class AuthenticateDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
