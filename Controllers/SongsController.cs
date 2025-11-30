using Microsoft.AspNetCore.Mvc;
using MusicAPI.DTOs;
using MusicAPI.Models;
using MusicAPI.Services;

namespace MusicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly ISongService _songService;

    public SongsController(ISongService songService)
    {
        _songService = songService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Song>>> GetAll()
    {
        var songs = await _songService.GetAllSongsAsync();
        return Ok(songs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Song>> GetById(int id)
    {
        var song = await _songService.GetSongByIdAsync(id);
        if (song == null) return NotFound();
        return Ok(song);
    }

    [HttpPost]
    public async Task<ActionResult<Song>> Create(CreateSongDto dto)
    {
        var song = new Song
        {
            Title = dto.Title,
            DurationInSeconds = dto.DurationInSeconds,
            AlbumId = dto.AlbumId
        };

        var created = await _songService.CreateSongAsync(song);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Song>> Update(int id, CreateSongDto dto)
    {
        var song = new Song
        {
            Title = dto.Title,
            DurationInSeconds = dto.DurationInSeconds,
            AlbumId = dto.AlbumId
        };

        var updated = await _songService.UpdateSongAsync(id, song);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _songService.DeleteSongAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
