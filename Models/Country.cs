using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class Country
{
    public int Id { get; set; }

    [Required]
    public required string CountryName { get; set; }

    public string? Img { get; set; }

    public ICollection<Brand> Brands { get; } = new List<Brand>();
}
