namespace GidraTopServer.Models;

public class ProductCard
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    public required string Name
    {
        get; set;
    }
    public required string Brand { get; set; }
    public required string Country { get; set; }
    public bool Availability { get; set; }
    public required string Price { get; set; }
}
