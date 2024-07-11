using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Migrations;


namespace GidraTopServer.Models;

public class Product
{
    
    public int Id { get; set; }

    [Required]
    public required string ProductName { get; set; }

    [Required]
    public int BrandId { get; set; }

    public Brand? Brand { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public int Availability { get; set; } = 0;

    [Required]
    public int Price { get; set; }

    public int Rating { get; set; } = 0;
    
    public ICollection<Rating>? Ratings { get; set; }

    public string? Img { get; set; }

    public ProductInfo? ProductInfo { get; set; }

    public string? type { get; set; }
}