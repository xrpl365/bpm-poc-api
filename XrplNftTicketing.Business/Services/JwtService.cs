using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using XrplNftTicketing.Business.Interfaces;
using XrplNftTicketing.Entities.Configurations;

namespace XrplNftTicketing.Business.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSetting;
        private readonly IIdentityService _identityService;

        
        public JwtService(JwtSettings jwtSetting, IIdentityService identityService)
        {
            _jwtSetting = jwtSetting;
            _identityService = identityService;
        }

        public string GenerateSecurityToken(string email, string password)
        {
            if (_identityService.Authenticate(email, password))
            {
                var roles = _identityService.GetUserRoles(email);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _jwtSetting.Issuer,
                    Subject = new ClaimsIdentity(new[]
                    {
                        // Custom Claims Can be added here
                        new Claim(ClaimTypes.Email, email)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpirationInMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                
                // Add roles to Claims
                foreach (var role in roles)
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }

            throw new Exception("Invalid login credentials.");
        }
    }
}
