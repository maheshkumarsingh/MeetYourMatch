using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; } = string.Empty;
    [Required(ErrorMessage = "KnownAs is required.")]
    public string? KnownAs { get; set; }
    [Required(ErrorMessage = "Gender is required.")]
    public string? Gender { get; set; }
    [Required(ErrorMessage = "DateOfBirth is required.")]
    public string? DateOfBirth { get; set; }
    [Required(ErrorMessage = "City is required.")]
    public string? City { get; set; }
    [Required(ErrorMessage = "Country is required.")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;

}
