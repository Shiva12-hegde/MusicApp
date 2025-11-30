namespace MusicAPI.DTOs;

public class CreateSongDto
{
    public string Title { get; set; } = string.Empty;
    public int DurationInSeconds { get; set; }
    public int AlbumId { get; set; }
}
