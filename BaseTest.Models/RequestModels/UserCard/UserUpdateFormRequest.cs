namespace BaseTest.Models.RequestModels.UserCard;

public class UserUpdateFormRequest
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }   
    public string? PhoneNumber { get; set; }
}