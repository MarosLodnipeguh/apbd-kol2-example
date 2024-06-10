using System.ComponentModel.DataAnnotations;

namespace apbd_kol2.DTOs;

public class NewOrderInfoDto
{
    [Required]
    public int employeeId { get; set; }
    [Required]
    public DateTime acceptedAt { get; set; }
    public string? comments { get; set; }
    [Required]
    public IEnumerable<NewOrderPastryDto> pastries { get; set; }
}