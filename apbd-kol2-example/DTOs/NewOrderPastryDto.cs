using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace apbd_kol2.DTOs;

public class NewOrderPastryDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public int Amount { get; set; }
    public string? Comment { get; set; }
}