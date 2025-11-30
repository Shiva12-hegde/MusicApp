using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;

namespace MusicAPI.Services;

public class AlbumService : IAlbumService
{
    private readonly MusicDbContext _context;

    public AlbumService(MusicDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Songs)
            .ToListAsync();
    }

    public async Task<Album?> GetAlbumByIdAsync(int id)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Songs)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Album> CreateAlbumAsync(Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task<Album?> UpdateAlbumAsync(int id, Album album)
    {
        var existing = await _context.Albums.FindAsync(id);
        if (existing == null) return null;

        existing.Title = album.Title;
        existing.ReleaseDate = album.ReleaseDate;
        existing.ArtistId = album.ArtistId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAlbumAsync(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return false;

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        return true;
    }
}
