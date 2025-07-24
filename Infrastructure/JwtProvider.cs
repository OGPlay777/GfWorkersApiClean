using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OperationWorker.Core.Abstractions.Auth;
using OperationWorker.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OperationWorker.Infrastructure
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateToken(AppUser appuser)
        {
            Claim[] claims = [new("userId", appuser.Id.ToString())];

            if (appuser.AccessLevel.Contains("Admin"))
            {
                claims = [new("userId", appuser.Id.ToString()), new("Admin", "True"), new("Office", "True"), new("Supervisor", "True"), new("Worker", "True")];
            }
            if (appuser.AccessLevel.Contains("Office"))
            {
                claims = [new("userId", appuser.Id.ToString()), new("Office", "True"), new("Supervisor", "True"), new("Worker", "True")];
            }
            if (appuser.AccessLevel.Contains("Supervisor"))
            {
                claims = [new("userId", appuser.Id.ToString()), new("Supervisor", "True"), new("Worker", "True")];
            }
            if (appuser.AccessLevel.Contains("Worker"))
            {
                claims = [new("userId", appuser.Id.ToString()), new("Worker", "True")];
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

    }
}
