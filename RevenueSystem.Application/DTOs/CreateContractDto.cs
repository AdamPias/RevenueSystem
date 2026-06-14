using System.ComponentModel.DataAnnotations;

namespace RevenueSystem.Application.DTOs;

public class CreateContractDto
{
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    public int SoftwareId { get; set; }
    
    [Required]
    public string SoftwareVersion { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    

    [Range(0, 3, ErrorMessage = "Dodatkowe wsparcie może wynosić od 0 do 3 lat.")]
    public int ExtraSupportYears { get; set; }
}