namespace PawsitiveVibesAPI.Models.Responses;

public class RegisterResponse : BaseResponse
{
    public User User { get; set; }

    public RegisterResponse(User user)
    {
        User = user;
    }

    public RegisterResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}