using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GidraTopServer.Models;

public class BasketProduct
{
    public int Id { get; set; }

    [Required]
    public int BasketId { get; set; }

    public required Basket Basket { get; set; }

    [Required]
    public int ProductId { get; set; }

    public required Product Product { get; set; }
}
