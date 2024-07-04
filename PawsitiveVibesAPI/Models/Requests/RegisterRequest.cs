using System.ComponentModel.DataAnnotations;

namespace PawsitiveVibesAPI.Models.Requests;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    
    public string ProfilePicture { get; set; }
    
    [Required]
    public string[] FavoriteAnimals { get; set; }
}