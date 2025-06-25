using API.Extensions;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser:BaseModel
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = "NA";
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public DateOnly DateOfBirth { get; set; } = DateOnly.MinValue;
    public required string KnownAs { get; set; } = "NA";
    [StringLength(10, MinimumLength =3)]
    public required string Gender { get; set; } = "Male";
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public required string? City { get; set; }
    public required string Country { get; set; }
    public List<Photo> Photos { get; set; } = []; //one to many relationship with Photo entity
    public List<UserLike> LikedByUsers { get; set; } = [];
    public List<UserLike> LikedUsers { get; set; } = [];
    public List<Message> MessagesSent { get; set; } = [];
    public List<Message> MessagesReceived { get; set; } = [];
}
