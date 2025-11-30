namespace MusicAPI.Models;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime? FormedDate { get; set; }
    public ICollection<Album> Albums { get; set; } = new List<Album>();
}
