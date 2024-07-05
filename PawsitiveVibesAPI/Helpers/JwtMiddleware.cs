using PawsitiveVibesAPI.Models;
using PawsitiveVibesAPI.Services;

namespace PawsitiveVibesAPI.Helpers;

public class JwtMiddleware(ILogger<JwtMiddleware> logger, JwtService jwtService)
{
    private readonly ILogger<JwtMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly RequestDelegate _next;
    private readonly JwtService _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    
    public async Task Invoke(HttpContext httpContext)
    {
        string token = httpContext.Request.Headers["Authorization"].SingleOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            User user = await _jwtService.ValidateTokenAsync(token);
            httpContext.Items["User"] = user;
        }

        await _next(httpContext);
    }
}