namespace PawsitiveVibesAPI.Repositories;

public static class SqlRepository
{
    public const string FindUserByUsername = "SELECT id, username, password FROM users WHERE username = @Username";
    public const string FindUserByEmailAddress = "SELECT id, emailAddress FROM users WHERE username = @Username";
    public const string GetFeedForUser = "SELECT p.*, u.id, u.username FROM posts p LEFT JOIN users u ON p.userId = u.id WHERE category IN @Category ORDER BY createdAt DESC";
    public const string GetFeedByUserId = "SELECT p.*, u.id, u.username FROM posts p LEFT JOIN sers u ON p.userId = u.id WHERE userId = @UserId ORDER BY createdAt DESC";
}