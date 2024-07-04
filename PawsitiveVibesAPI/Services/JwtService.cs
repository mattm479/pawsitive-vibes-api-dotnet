using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PawsitiveVibesAPI.Models;
using PawsitiveVibesAPI.Repositories;

namespace PawsitiveVibesAPI.Services;

public class JwtService(IConfiguration configuration, IUserRepository userRepository)
{
    private readonly JwtOptions _jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    
    public Task<string> GenerateTokenAsync(string userId, TimeSpan expiration, string[] permissions)
    {

        byte[] keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
        SymmetricSecurityKey symmetricSecurityKey = new(keyBytes);
        SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new List<Claim>
        {
            new("jti", userId),
            new("aud", _jwtOptions.Audience)
        };
        var roleClaims = permissions.Select(x => new Claim(ClaimTypes.Role, x));
        claims.AddRange(roleClaims);
        
        JwtSecurityToken token = new(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.Add(expiration),
            signingCredentials: signingCredentials
        );

        string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        return Task.FromResult(jwtToken);
    }

    public async Task<User> ValidateTokenAsync(string jwtToken)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
        SymmetricSecurityKey symmetricSecurityKey = new(keyBytes);
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = symmetricSecurityKey,
            ValidIssuer = _jwtOptions.Issuer,
            RequireExpirationTime = true
        };
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        TokenValidationResult result = await jwtSecurityTokenHandler.ValidateTokenAsync(jwtToken, tokenValidationParameters);
        if (!result.IsValid)
        {
            return null;
        }

        JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
        Claim userIdClaim = jwtSecurityToken.Claims.SingleOrDefault(c => c.ValueType == "jti");
        if (userIdClaim == null)
        {
            return null;
        }
        
        string userId = userIdClaim.Value;
        User user = await _userRepository.FindUserByIdAsync(userId);
        
        // TODO: Validate user role(s) against route role(s)
        return user;
    }
}