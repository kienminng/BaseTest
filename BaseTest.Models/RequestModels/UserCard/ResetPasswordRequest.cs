using System.ComponentModel.DataAnnotations;

namespace BaseTest.Models.RequestModels.UserCard;

public class ResetPasswordRequest
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Code { get; set; }
    [Required]
    public string? NewPassword { get; set; }
    
}