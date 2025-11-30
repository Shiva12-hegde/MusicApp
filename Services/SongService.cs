using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;

namespace MusicAPI.Services;

public class SongService : ISongService
{
    private readonly MusicDbContext _context;

    public SongService(MusicDbContext context)
    {
        _context = context;
    }

   public async Task<IEnumerable<Song>> GetAllSongsAsync()
{
    return await _context.Songs
        .Include(s => s.Album)
        .ThenInclude(a => a!.Artist)
        .ToListAsync();
}


    public async Task<Song?> GetSongByIdAsync(int id)
{
    return await _context.Songs
        .Include(s => s.Album)
        .ThenInclude(a => a!.Artist)
        .FirstOrDefaultAsync(s => s.Id == id);
}

    public async Task<Song> CreateSongAsync(Song song)
    {
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();
        return song;
    }

    public async Task<Song?> UpdateSongAsync(int id, Song song)
    {
        var existing = await _context.Songs.FindAsync(id);
        if (existing == null) return null;

        existing.Title = song.Title;
        existing.DurationInSeconds = song.DurationInSeconds;
        existing.AlbumId = song.AlbumId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteSongAsync(int id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null) return false;

        _context.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }
}
