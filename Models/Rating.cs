namespace GidraTopServer.Models;

public class Rating
{
    public int Id { get; set; }

    public int Rate { get; set; }

    public int UserId { get; set; }

    public required User User { get; set; }

    public int ProductId { get; set; }

    public required Product Product { get; set; }
}
