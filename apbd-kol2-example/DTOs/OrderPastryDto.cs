using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace apbd_kol2.DTOs;

public class OrderPastryDto
{
    [Required]
    public string Name { get; set; }
    
    [Required] [DataType("decimal")] [Precision(10, 2)]
    public decimal Price { get; set; }
    
    [Required]
    public int Amount { get; set; }
    public string? Comment { get; set; }
}