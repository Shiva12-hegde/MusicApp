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
                    .Select((file, index) => new
                    {
                        id = index + 1,
                        title = Path.GetFileNameWithoutExtension(file),
                        url = "/music/" + Path.GetFileName(file)
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
    }
}
