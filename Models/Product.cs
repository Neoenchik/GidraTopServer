using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GidraTopServer.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int BrandId { get; set; }

    public required Brand Brand { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public required Category Category { get; set; }

    public int Availability { get; set; }

    public int Price { get; set; }

    [Required]
    public int Rating { get; set; } = 0;
    
    public ICollection<Rating>? Ratings { get; set; }

    public string? Img { get; set; }

    public ProductInfo? ProductInfo { get; set; }
}