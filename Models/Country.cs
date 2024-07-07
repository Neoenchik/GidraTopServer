namespace GidraTopServer.Models;

public class Country
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Img { get; set; }

    public required ICollection<Brand> Brands { get; set; }
}
