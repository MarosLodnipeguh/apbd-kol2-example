using System.ComponentModel.DataAnnotations;

namespace apbd_kol2.DTOs;

public class OrderInfoDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public DateTime AcceptedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public string? Comments { get; set; }
    [Required]
    public IEnumerable<OrderPastryDto> pastries { get; set; }
}