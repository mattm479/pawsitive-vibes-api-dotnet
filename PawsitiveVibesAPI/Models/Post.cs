namespace PawsitiveVibesAPI.Models;

public class Post : BaseModel
{
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public string Image { get; set; }
    
    public string Category { get; set; }
    
    public string UserId { get; set; }
}