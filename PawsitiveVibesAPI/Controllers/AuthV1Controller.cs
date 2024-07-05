using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawsitiveVibesAPI.Models.Requests;
using PawsitiveVibesAPI.Models.Responses;
using PawsitiveVibesAPI.Services;

namespace PawsitiveVibesAPI.Controllers;

[Route("auth/v1")]
[Produces("application/json")]
[ApiController]
public class AuthV1Controller(ILogger<AuthV1Controller> logger, IAuthService authService) : ControllerBase
{
    private readonly ILogger<AuthV1Controller> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        RegisterResponse response = await _authService.RegisterUserAsync(request, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        LoginResponse response = await _authService.LoginAsync(request, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }

    [HttpPost("forgot-username")]
    public async Task<ActionResult<ForgotUsernameResponse>> ForgotUsernameAsync([FromBody] ForgotUsernameRequest request, CancellationToken cancellationToken)
    {
        ForgotUsernameResponse response = await _authService.ForgotUsernameAsync(request, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        ForgotPasswordResponse response = await _authService.ForgotPasswordAsync(request, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpPost("user-profile/{userId:length(36)}")]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfileByIdAsync([FromRoute] string userId, CancellationToken cancellationToken)
    {
        UserProfileResponse response = await _authService.GetUserProfileByIdAsync(userId, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }
}