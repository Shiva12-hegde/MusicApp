using MusicAPI.Models;

namespace MusicAPI.Services;

public interface ISongService
{
    Task<IEnumerable<Song>> GetAllSongsAsync();
    Task<Song?> GetSongByIdAsync(int id);
    Task<Song> CreateSongAsync(Song song);
    Task<Song?> UpdateSongAsync(int id, Song song);
    Task<bool> DeleteSongAsync(int id);
}
