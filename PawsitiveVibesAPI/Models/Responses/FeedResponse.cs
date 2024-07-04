namespace PawsitiveVibesAPI.Models.Responses;

public class FeedResponse : BaseResponse
{
    public IEnumerable<Post> Posts { get; set; }

    public FeedResponse(IEnumerable<Post> posts)
    {
        Posts = posts;
    }

    public FeedResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}