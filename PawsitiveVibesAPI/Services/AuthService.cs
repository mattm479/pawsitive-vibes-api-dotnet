using PawsitiveVibesAPI.Models;
using PawsitiveVibesAPI.Models.Requests;
using PawsitiveVibesAPI.Models.Responses;
using PawsitiveVibesAPI.Repositories;

namespace PawsitiveVibesAPI.Services;

public interface IAuthService
{
    Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<ForgotUsernameResponse> ForgotUsernameAsync(ForgotUsernameRequest request, CancellationToken cancellationToken);
    Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken);
}

public class AuthService(ILogger<AuthService> logger, IUserRepository userRepository, JwtService jwtService) : IAuthService
{
    private readonly ILogger<AuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly JwtService _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));

    public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        User user = new(request);
        bool wasCreateSuccessful = await _userRepository.CreateUserAsync(user, cancellationToken);

        return wasCreateSuccessful 
            ? new RegisterResponse(user) 
            : new RegisterResponse("Unable to create user. Please try again.");
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByUsernameAsync(request.Username, cancellationToken);
        if (user == null)
        {
            return new LoginResponse("User does not exist");
        }
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        request.Password = string.Empty;
        
        if (!isPasswordValid)
        {
            return new LoginResponse("Invalid password");
        }

        // Generate JWT token and return LoginResponse with UserId and JwtToken
        string jwtToken = await _jwtService.GenerateTokenAsync(user.Id, TimeSpan.Zero, []);

        return new LoginResponse(user.Id, jwtToken);
    }

    public async Task<ForgotUsernameResponse> ForgotUsernameAsync(ForgotUsernameRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByEmailAddressAsync(request.EmailAddress, cancellationToken);
        
        return user == null 
            ? new ForgotUsernameResponse("User does not exist") 
            : new ForgotUsernameResponse(user);
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.FindUserByUsernameAsync(request.Username, cancellationToken);
        if (user == null)
        {
            return new ForgotPasswordResponse(errorMessage: "User does not exist");
        }

        string temporaryPassword = GenerateRandomPassword();
        user.TemporaryPassword = temporaryPassword;
        
        await _userRepository.UpdateUserAsync(user, cancellationToken);

        return new ForgotPasswordResponse(temporaryPassword: temporaryPassword);
    }
    
    private static string GenerateRandomPassword()
    {
        const int numbersLength = 12;
        const string options = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string temporaryPassword = string.Empty;
        Random rnd = new();

        for (int i = 1; i <= numbersLength; i++)
        {
            int index = rnd.Next(options.Length);
            temporaryPassword += options[index];
        }

        return temporaryPassword;
    }
}
