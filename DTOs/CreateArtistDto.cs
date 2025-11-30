namespace MusicAPI.DTOs;

public class CreateArtistDto
{
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime? FormedDate { get; set; }
}
