using Microsoft.AspNetCore.Mvc;
using MusicAPI.DTOs;
using MusicAPI.Models;
using MusicAPI.Services;

namespace MusicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistsController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artist>>> GetAll()
    {
        var artists = await _artistService.GetAllArtistsAsync();
        return Ok(artists);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artist>> GetById(int id)
    {
        var artist = await _artistService.GetArtistByIdAsync(id);
        if (artist == null) return NotFound();
        return Ok(artist);
    }

    [HttpPost]
    public async Task<ActionResult<Artist>> Create(CreateArtistDto dto)
    {
        var artist = new Artist
        {
            Name = dto.Name,
            Genre = dto.Genre,
            Country = dto.Country,
            FormedDate = dto.FormedDate
        };

        var created = await _artistService.CreateArtistAsync(artist);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Artist>> Update(int id, CreateArtistDto dto)
    {
        var artist = new Artist
        {
            Name = dto.Name,
            Genre = dto.Genre,
            Country = dto.Country,
            FormedDate = dto.FormedDate
        };

        var updated = await _artistService.UpdateArtistAsync(id, artist);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _artistService.DeleteArtistAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
