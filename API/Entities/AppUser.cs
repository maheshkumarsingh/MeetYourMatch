using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser:BaseModel
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = "NA";
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}
