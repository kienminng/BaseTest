using System.ComponentModel.DataAnnotations;

namespace BaseTest.Models.RequestModels.UserCard;

public class ChangePasswordRequest
{
    [Required]
    public string? OldPassword { get; set; }
    [Required]
    public string? NewPassword { get; set; }
}