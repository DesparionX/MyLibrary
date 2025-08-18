using Microsoft.IdentityModel.Tokens;
using MyLibrary.Server.Configs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Handlers.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyLibrary.Server.Handlers
{
    public class JWTGenerator : IJWTGenerator
    {
        public Task<string> GenerateJWT(IUser<string> user)
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName!)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: JwtConfig.JwtIssuer,
                audience: JwtConfig.JwtAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}
