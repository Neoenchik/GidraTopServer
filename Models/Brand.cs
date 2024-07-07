using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class Brand
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int CountryId { get; set; }

    public required Country Country { get; set; }

    // Навигационное свойство для связи один ко многим с Product
    public required ICollection<Product> Products { get; set; }

    public required ICollection<Category> Categories { get; set; }
}