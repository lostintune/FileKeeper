using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FileKeeper.Infrastructure.Auth;

public class JwtTokenGenerator: IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(UserEntity user)
    {
        // Step 1 — Claims (assertions about the user)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
        };
        
        // Step 2 — Secret key and signature
        var secretKey = _configuration["JwtOptions:SecretKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // Step 3 - Collect the token itself
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtOptions:Issuer"],
            audience: _configuration["JwtOptions:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );
        
        // Step 4 — Convert the token object into a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}