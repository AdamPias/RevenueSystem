using System.ComponentModel.DataAnnotations;

namespace RevenueSystem.Application.DTOs;

public class CreatePaymentDto
{
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Kwota musi być większa niż zero.")]
    public decimal Amount { get; set; }
}