namespace MusicAPI.DTOs;

public class CreateAlbumDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public int ArtistId { get; set; }
}
