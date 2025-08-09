using Maqha.Utilities.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Maqha.Utilities.Helpers
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Claudia 
        public string GenerateToken(TokenDTO tokenDTO)
        {
            if (tokenDTO == null)
                throw new ArgumentNullException(nameof(tokenDTO));

            if (string.IsNullOrEmpty(tokenDTO.Email) || string.IsNullOrEmpty(tokenDTO.Role))
                throw new ArgumentException("Email and Role are required");
            var JWTSettings = _configuration.GetSection("JWT");
            var validIssuer = JWTSettings["ValidIssuer"];
            var validAudience = JWTSettings["ValidAudience"];
            var secretKey = JWTSettings["Key"];
            var claims = new List<Claim>
    {
        new Claim("emil", tokenDTO.Email),
        new Claim(ClaimTypes.Role, tokenDTO.Role),
        new Claim("id", tokenDTO.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat,
            new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
            ClaimValueTypes.Integer64)
    };

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //public string GenerateToken(TokenDTO tokenDTO)
        //{
        //    var claims = new List<Claim> 
        //    {
        //            new Claim("_e", tokenDTO.Email),
        //            new Claim(ClaimTypes.Role, tokenDTO.Role),
        //            new Claim("_u", tokenDTO.Id),
        //             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };
        //     var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])); // Use a secure key
        //    var Token = new JwtSecurityToken(
        //  issuer: _configuration["JWT:ValidIssuer"], // Issuer of the token
        //  audience: _configuration["JWT:ValidAudience"], // Audience for the token
        //  claims: claims, // Claims to include in the token
        //  expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:ExpirationInDays"])), // Token expiration time
        //  signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256) // Signing credentials using HMAC SHA256
        //  );
        //    return new JwtSecurityTokenHandler().WriteToken(Token);
        //}
    }
}
