using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;

namespace MusicAPI.Services;

public class ArtistService : IArtistService
{
    private readonly MusicDbContext _context;

    public ArtistService(MusicDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
    {
        return await _context.Artists
            .Include(a => a.Albums)
            .ToListAsync();
    }

    public async Task<Artist?> GetArtistByIdAsync(int id)
    {
        return await _context.Artists
            .Include(a => a.Albums)
            .ThenInclude(al => al.Songs)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Artist> CreateArtistAsync(Artist artist)
    {
        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();
        return artist;
    }

    public async Task<Artist?> UpdateArtistAsync(int id, Artist artist)
    {
        var existing = await _context.Artists.FindAsync(id);
        if (existing == null) return null;

        existing.Name = artist.Name;
        existing.Genre = artist.Genre;
        existing.Country = artist.Country;
        existing.FormedDate = artist.FormedDate;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteArtistAsync(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false;

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
        return true;
    }
}
