using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GidraTopServer.Models;

public class Basket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    //навигационное свойство для продуктов в корзине
    public ICollection<BasketProduct>? Basket_Products { get; set; }
}
