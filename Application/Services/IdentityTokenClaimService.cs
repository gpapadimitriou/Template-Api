using Application.Common.Models;
using Application.Interfaces;
using Application.Specifications.User;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Application.Common.Models.Config;

namespace Application.Services
{
    public class IdentityTokenClaimService : ITokenClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAsyncEfRepository<RefreshToken> _refreshTokenRepository;
        private readonly IAsyncEfRepository<Domain.Entities.User> _userRepository;
        private JwtConfiguration _jwtModel { get; }


        public IdentityTokenClaimService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
             IOptions<JwtConfiguration> jwtConfiguration,
          IAsyncEfRepository<RefreshToken> refreshTokenRepository,
          IAsyncEfRepository<Domain.Entities.User> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtModel = jwtConfiguration.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResponse> GetTokenAsync(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtModel.Secret);

            var user = new ApplicationUser();
            IList<string> roles = new List<string>();
            var claims = new List<Claim>();


            user = await _userManager.FindByNameAsync(email);

            roles = await _userManager.GetRolesAsync(user);
            claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim("UserId", user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.Now.Add(_jwtModel.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                ApplicationUserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            try
            {
                await _refreshTokenRepository.AddAsync(refreshToken);
                var entityUser = await _userRepository.FirstOrDefaultAsync(new UserSpecification(user.Id));
                entityUser.LastLogin = DateTime.UtcNow;
                await _userRepository.UpdateAsync(entityUser);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            var authResponse = new Common.Models.AuthenticationResponse()
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Id.ToString(),
            };
            return authResponse;
        }

        public async Task<AuthenticationResponse> RefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken is null)
            {
                throw new Exception("Token validation failed");
            }
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                throw new Exception("Token is not expired yet");
            }
            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.GetByIdAsync(int.Parse(refreshToken));

            if (storedRefreshToken is null)
            {
                throw new Exception("Refresh token not found");
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                throw new Exception("Refresh token expired");
            }

            if (storedRefreshToken.Invalidated)
            {
                throw new Exception("Invalidated refresh token");

            }

            if (storedRefreshToken.Used)
            {
                throw new Exception("Refresh token has already been used");
            }

            if (storedRefreshToken.JwtId != jti)
            {
                throw new Exception("Token does not belong to this refresh token");
            }

            storedRefreshToken.Used = true;
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "UserId").Value);
            return await GetTokenAsync(user.UserName);
        }

        public async Task<ApplicationUser> GetApplicationUser(string email)
        {
            return await _userManager.FindByNameAsync(email);
        }

        public async Task<bool> AreCredentialsValid(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, true);
            return result.Succeeded;
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = Encoding.ASCII.GetBytes(_jwtModel.Secret);

                var tokenValidationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };
                var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<string> CreateIdentityUser(string email, string password, string role)
        {
            var applicationUser = new ApplicationUser { UserName = email, isActive = true, DateInserted = DateTime.UtcNow, LockoutEnabled = false };
            await _userManager.CreateAsync(applicationUser, password);
            await _userManager.AddToRoleAsync(applicationUser, role);
            return applicationUser.Id;
        }

        public async Task<List<string>> GetAllRoles()
        {
            return await _roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task<List<string>> GetRoles(string email)
        {
            var test = await _userManager.GetRolesAsync(await GetApplicationUser(email));
            return test.ToList();
        }
    }
}
