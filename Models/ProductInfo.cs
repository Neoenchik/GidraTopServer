using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class ProductInfo
{
    public int Id { get; set; }

    
    public int ProductId { get; set; }

    public Product? Product { get; set; }


    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }
}
