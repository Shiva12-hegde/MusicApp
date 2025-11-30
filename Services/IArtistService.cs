using MusicAPI.Models;

namespace MusicAPI.Services;

public interface IArtistService
{
    Task<IEnumerable<Artist>> GetAllArtistsAsync();
    Task<Artist?> GetArtistByIdAsync(int id);
    Task<Artist> CreateArtistAsync(Artist artist);
    Task<Artist?> UpdateArtistAsync(int id, Artist artist);
    Task<bool> DeleteArtistAsync(int id);
}
