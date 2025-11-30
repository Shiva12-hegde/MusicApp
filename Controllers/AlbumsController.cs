using Microsoft.AspNetCore.Mvc;
using MusicAPI.DTOs;
using MusicAPI.Models;
using MusicAPI.Services;

namespace MusicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumsController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Album>>> GetAll()
    {
        var albums = await _albumService.GetAllAlbumsAsync();
        return Ok(albums);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetById(int id)
    {
        var album = await _albumService.GetAlbumByIdAsync(id);
        if (album == null) return NotFound();
        return Ok(album);
    }

    [HttpPost]
    public async Task<ActionResult<Album>> Create(CreateAlbumDto dto)
    {
        var album = new Album
        {
            Title = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            ArtistId = dto.ArtistId
        };

        var created = await _albumService.CreateAlbumAsync(album);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Album>> Update(int id, CreateAlbumDto dto)
    {
        var album = new Album
        {
            Title = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            ArtistId = dto.ArtistId
        };

        var updated = await _albumService.UpdateAlbumAsync(id, album);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _albumService.DeleteAlbumAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
