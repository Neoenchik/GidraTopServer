using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    // Навигационное свойство для связи один ко многим с Product
    public ICollection<Product>? Products { get; set; }

    public ICollection<Brand>? Brands { get; set; }
}