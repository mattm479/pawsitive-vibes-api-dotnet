using PawsitiveVibesAPI.Models;
using PawsitiveVibesAPI.Models.Responses;
using PawsitiveVibesAPI.Repositories;

namespace PawsitiveVibesAPI.Services;

public interface IPostService
{
    Task<FeedResponse> GetFeedForUserAsync(string userId, CancellationToken cancellationToken);
    Task<FeedResponse> GetFeedByUserIdAsync(string userId, CancellationToken cancellationToken);
}

public class PostService(ILogger<PostService> logger, IPostRepository postRepository, IUserRepository userRepository) : IPostService
{
    private readonly ILogger<PostService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPostRepository _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    
    public async Task<FeedResponse> GetFeedForUserAsync(string userId, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return new FeedResponse("User does not exist");
        }
        
        IEnumerable<Post> posts = await _postRepository.GetFeedForUserAsync(user.FavoriteAnimals, cancellationToken);
        
        return !posts.Any() 
            ? new FeedResponse("No posts to display") 
            : new FeedResponse(posts);
    }
    
    public async Task<FeedResponse> GetFeedByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return new FeedResponse("User does not exist");
        }
        
        IEnumerable<Post> posts = await _postRepository.GetFeedByUserIdAsync(userId, cancellationToken);
        
        return !posts.Any() 
            ? new FeedResponse("No posts to display") 
            : new FeedResponse(posts);
    }
}