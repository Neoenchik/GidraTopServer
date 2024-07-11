using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class Brand
{
    public int Id { get; set; }

    [Required]
    public required string BrandName { get; set; }

    [Required]
    public int CountryId { get; set; }

    // Навигационное свойство для связи один ко многим с Product
    public ICollection<Product>? Products { get; set; }

    public ICollection<Category>? Categories { get; set; }
}