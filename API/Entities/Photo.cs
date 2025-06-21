using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo : BaseModel
{
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    // Navigation property to AppUser
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}