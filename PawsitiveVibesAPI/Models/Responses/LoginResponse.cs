namespace PawsitiveVibesAPI.Models.Responses;

public class LoginResponse : BaseResponse
{
    public string UserId { get; set; } = string.Empty;

    public string JwtToken { get; set; } = string.Empty;

    public LoginResponse(string userId, string jwtToken)
    {
        UserId = userId;
        JwtToken = jwtToken;
    }
    
    public LoginResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}