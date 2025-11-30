using MusicAPI.Models;

namespace MusicAPI.Services;

public interface IAlbumService
{
    Task<IEnumerable<Album>> GetAllAlbumsAsync();
    Task<Album?> GetAlbumByIdAsync(int id);
    Task<Album> CreateAlbumAsync(Album album);
    Task<Album?> UpdateAlbumAsync(int id, Album album);
    Task<bool> DeleteAlbumAsync(int id);
}
