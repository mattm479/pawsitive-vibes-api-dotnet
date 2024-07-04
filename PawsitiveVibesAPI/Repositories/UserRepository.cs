using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using PawsitiveVibesAPI.Models;

namespace PawsitiveVibesAPI.Repositories;

public interface IUserRepository
{
    Task<User> FindUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<User> FindUserByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User> FindUserByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken);
    Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
}

public class UserRepository(ILogger<UserRepository> logger, IConfiguration configuration) : IUserRepository
{
    private readonly ILogger<UserRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly string _connectionString = configuration.GetConnectionString("Default");

    public async Task<User> FindUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
    
        User user = await connection.GetAsync<User>(userId);
        
        await connection.CloseAsync();
        
        return user;
    }

    public async Task<User> FindUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);

        User user = await connection.QuerySingleOrDefaultAsync<User>(SqlRepository.FindUserByUsername, new { Username = username });

        await connection.CloseAsync();
        
        return user;
    }

    public async Task<User> FindUserByEmailAddressAsync(string username, CancellationToken cancellationToken)
    {
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);

        User user = await connection.QuerySingleOrDefaultAsync<User>(SqlRepository.FindUserByEmailAddress, new { Username = username });

        await connection.CloseAsync();
        
        return user;
    }

    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        bool isCompleted = false;
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        NpgsqlTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            int rowsAffected = await connection.InsertAsync(user, transaction);
            if (rowsAffected == 1)
            {
                await transaction.CommitAsync(cancellationToken);
                isCompleted = true;
            }

            return isCompleted;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (isCompleted == false)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            await transaction.DisposeAsync();
            await connection.CloseAsync();
        }
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        bool isCompleted = false;
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        NpgsqlTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            bool isUpdated = await connection.UpdateAsync(user, transaction);
            if (isUpdated)
            {
                await transaction.CommitAsync(cancellationToken);
                isCompleted = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (isCompleted == false)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            await transaction.DisposeAsync();
            await connection.CloseAsync();
        }
    }
    
    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
    {
        bool isCompleted = false;
        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        NpgsqlTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            bool isUpdated = await connection.DeleteAsync(user, transaction);
            if (isUpdated)
            {
                await transaction.CommitAsync(cancellationToken);
                isCompleted = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (isCompleted == false)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            await transaction.DisposeAsync();
            await connection.CloseAsync();
        }
    }
}