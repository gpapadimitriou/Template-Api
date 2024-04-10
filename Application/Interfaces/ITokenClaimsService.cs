using Application.Common.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<AuthenticationResponse> RefreshToken(string token, string refreshToken);

        Task<AuthenticationResponse> GetTokenAsync(string email);

        Task<ApplicationUser> GetApplicationUser(string email);

        Task<bool> AreCredentialsValid(string email, string password);

        Task<string> CreateIdentityUser(string email, string password, string role);

        Task<List<string>> GetAllRoles();

        Task<List<string>> GetRoles(string email);
    }
}
