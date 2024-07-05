using System.Text.Json.Serialization;
using PawsitiveVibesAPI.Models.Requests;

namespace PawsitiveVibesAPI.Models;

public class User(string id, string username, string password) : BaseModel
{
    public new string Id { get; set; } = id;

    public string Username { get; set; } = username;

    public string EmailAddress { get; set; }

    [JsonIgnore]
    public string Password { get; set; } = password;

    [JsonIgnore]
    public string TemporaryPassword { get; set; } = string.Empty;

    public string ProfilePicture { get; set; } = string.Empty;
    
    public IEnumerable<string> FavoriteAnimals { get; set; }

    public User(RegisterRequest request) : this(Guid.NewGuid().ToString(), request.Username, BCrypt.Net.BCrypt.HashPassword(request.Password))
    {
        EmailAddress = request.EmailAddress;
        ProfilePicture = request.ProfilePicture;
        FavoriteAnimals = request.FavoriteAnimals;
    }
}