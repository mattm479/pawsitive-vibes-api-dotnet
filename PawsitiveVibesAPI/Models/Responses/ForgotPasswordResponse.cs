namespace PawsitiveVibesAPI.Models.Responses;

public class ForgotPasswordResponse(string temporaryPassword = null, string errorMessage = null) : BaseResponse
{
    public string TemporaryPassword { get; set; } = temporaryPassword;

    public new string ErrorMessage { get; set; } = errorMessage;
}