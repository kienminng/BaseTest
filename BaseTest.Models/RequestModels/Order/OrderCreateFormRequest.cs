using System.ComponentModel.DataAnnotations;
using BaseTest.Models.ReponseModels.OrderDetails;

namespace BaseTest.Models.ReponseModels.Order;

public class OrderCreateFormRequest
{
    public string? FullName { get; set; }
    [Required(ErrorMessage = "phone number can not be null")]
    [MinLength(11)]
    [MaxLength(13)]
    public string? PhoneNumber { get; set; }
    [Required(ErrorMessage = "Address can not be null")]
    public string? Address { get; set; }
    public List<OrderDetailCreateFormRequest>? OrderDetails { get; set; }
}