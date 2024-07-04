using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawsitiveVibesAPI.Models.Responses;
using PawsitiveVibesAPI.Services;

namespace PawsitiveVibesAPI.Controllers;

[Route("post/v1/posts")]
[Produces("application/json")]
[ApiController]
public class PostV1Controller(ILogger<PostV1Controller> logger, IPostService postService) : ControllerBase
{
    private readonly ILogger<PostV1Controller> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPostService _postService = postService ?? throw new ArgumentNullException(nameof(postService));

    [Authorize]
    [HttpGet("{userId:length(36)}/feed")]
    public async Task<ActionResult<FeedResponse>> GetFeedForUserAsync([FromRoute] string userId, CancellationToken cancellationToken)
    {
        FeedResponse response = await _postService.GetFeedForUserAsync(userId, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpGet("{userId:length(36)}")]
    public async Task<ActionResult<FeedResponse>> GetFeedByUserIdAsync([FromRoute] string userId, CancellationToken cancellationToken)
    {
        FeedResponse response = await _postService.GetFeedByUserIdAsync(userId, cancellationToken);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }
}