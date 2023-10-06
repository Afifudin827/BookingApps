using Microsoft.IdentityModel.Tokens;
using Server.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Utilities.Handler;

public class TokenHendler : ITokenHendler
{
    private readonly IConfiguration _configuration;
    public TokenHendler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string Generate(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTServices:SecretKey"]));
        var sigingCredential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOption = new JwtSecurityToken(issuer: _configuration["JWTServices:Issuer"],
                                                audience: _configuration["JWTServices:Audience"],
                                                claims: claims,
                                                expires: DateTime.Now.AddMinutes(5),
                                                signingCredentials: sigingCredential);
        var encoderToken = new JwtSecurityTokenHandler().WriteToken(tokenOption);
        return encoderToken;
    }
}
