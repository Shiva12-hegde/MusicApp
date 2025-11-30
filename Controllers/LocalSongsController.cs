using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalSongsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLocalSongs()
        {
            try
            {
                // Content root is C:\MusicAPI (you see that in the console logs)
                var contentRoot = Directory.GetCurrentDirectory();
                var musicFolder = Path.Combine(contentRoot, "wwwroot", "music");

                // Make sure folder exists
                if (!Directory.Exists(musicFolder))
                {
                    Directory.CreateDirectory(musicFolder);
                    // No files yet, return empty list
                    return Ok(Array.Empty<object>());
                }

                // Get all .mp3 files in that folder
                var files = Directory
                    .GetFiles(musicFolder, "*.mp3", SearchOption.TopDirectoryOnly)
                    .Select((file, index) => {
                        var fileName = Path.GetFileName(file);
                        return new
                        {
                            id = index + 1,
                            title = Path.GetFileNameWithoutExtension(file),
                            url = "/api/LocalSongs/file?name=" + Uri.EscapeDataString(fileName)
                        };
                    })
                    .ToList();

                return Ok(files);
            }
            catch (Exception ex)
            {
                // TEMP: If something goes wrong, tell the frontend the error message
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("file")]
        public IActionResult GetSongFile([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(new { error = "File name is required" });
                }

                var contentRoot = Directory.GetCurrentDirectory();
                var musicFolder = Path.Combine(contentRoot, "wwwroot", "music");
                
                // Normalize paths for security check
                var normalizedMusicFolder = Path.GetFullPath(musicFolder);
                var filePath = Path.GetFullPath(Path.Combine(musicFolder, name));

                // Security: Ensure the file is actually in the music folder (prevent directory traversal)
                if (!filePath.StartsWith(normalizedMusicFolder, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "Invalid file path" });
                }

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { error = "File not found" });
                }

                // Check if it's an MP3 file
                if (!Path.GetExtension(filePath).Equals(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "Invalid file type" });
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var contentType = "audio/mpeg";
                
                return File(fileStream, contentType, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
