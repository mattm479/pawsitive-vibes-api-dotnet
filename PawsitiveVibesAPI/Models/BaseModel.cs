using System.ComponentModel.DataAnnotations;

namespace PawsitiveVibesAPI.Models;

public class BaseModel
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string UpdatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string CreatedBy { get; set; } = string.Empty;
}