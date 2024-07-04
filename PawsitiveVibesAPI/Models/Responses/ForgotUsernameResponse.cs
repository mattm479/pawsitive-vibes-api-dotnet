namespace PawsitiveVibesAPI.Models.Responses;

public class ForgotUsernameResponse : BaseResponse
{
    public string Username { get; set; }
    
    public ForgotUsernameResponse(User user)
    {
        Username = user.Username;
    }

    public ForgotUsernameResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}