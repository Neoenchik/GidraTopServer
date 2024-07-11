namespace GidraTopServer.Models;

public class Banner
{
    public int Id { get; set; }

    public string? ImageUrl{ get; set; }

    public string Text { get; set; } = "";

    public string ButtonText { get; set; } = "Подробнее";

    public string ButtonLink { get; set; } = "#";
}
