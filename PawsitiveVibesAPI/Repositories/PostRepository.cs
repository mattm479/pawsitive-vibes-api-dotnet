using Dapper;
using Npgsql;
using PawsitiveVibesAPI.Models;

namespace PawsitiveVibesAPI.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetFeedForUserAsync(IEnumerable<string> favoriteAnimals, CancellationToken cancellationToken);
}

public class PostRepository(ILogger<PostRepository> logger, IConfiguration configuration) : IPostRepository
{
    private readonly ILogger<PostRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly string _connectionString = configuration.GetConnectionString("Default");
    
    public async Task<IEnumerable<Post>> GetFeedForUserAsync(IEnumerable<string> favoriteAnimals, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);

        IEnumerable<Post> posts = await connection.QueryAsync<Post>(SqlRepository.GetFeedForUser, new { Category = favoriteAnimals });

        await connection.CloseAsync();
        
        return posts;
    }
}