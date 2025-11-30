using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;

namespace MusicAPI.Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Song> Songs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>()
            .HasMany(a => a.Albums)
            .WithOne(al => al.Artist)
            .HasForeignKey(al => al.ArtistId);

        modelBuilder.Entity<Album>()
            .HasMany(al => al.Songs)
            .WithOne(s => s.Album)
            .HasForeignKey(s => s.AlbumId);
    }
}
