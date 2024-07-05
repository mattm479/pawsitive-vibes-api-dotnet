namespace PawsitiveVibesAPI.Models.Responses;

public class UserProfileResponse : BaseResponse
{
    public User User { get; set; }
    
    public IEnumerable<Post> Posts { get; set; }

    public UserProfileResponse(User user, IEnumerable<Post> posts)
    {
        User = user;
        Posts = posts;
    }

    public UserProfileResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}