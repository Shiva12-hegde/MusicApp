namespace MusicAPI.Models;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInSeconds { get; set; }
    public int AlbumId { get; set; }
    public Album? Album { get; set; }
}
