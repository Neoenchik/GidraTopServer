using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class ProductInfo
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    public required Product Product { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }
}
