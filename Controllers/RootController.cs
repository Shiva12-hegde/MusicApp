using Microsoft.AspNetCore.Mvc;

namespace MusicAPI.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    [HttpGet]
    public ContentResult Get()
    {
        var html = @"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"" />
  <title>My Music Player</title>
  <style>
    :root {
      --bg: #020617;
      --bg-panel: rgba(15, 23, 42, 0.9);
      --border: rgba(148, 163, 184, 0.25);
      --accent: #22c55e;
      --accent-soft: rgba(34, 197, 94, 0.12);
      --text-main: #e5e7eb;
      --text-muted: #9ca3af;
    }

    * {
      box-sizing: border-box;
    }

    body {
      margin: 0;
      font-family: system-ui, -apple-system, BlinkMacSystemFont, ""Segoe UI"", sans-serif;
      background: radial-gradient(circle at top, #0f172a 0, #020617 45%, #000 100%);
      color: var(--text-main);
      min-height: 100vh;
    }

    .app {
      padding: 24px 32px;
    }

    .app-header {
      display: flex;
      gap: 12px;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
    }

    .app-header h1 {
      margin: 0;
      font-size: 1.8rem;
      letter-spacing: 0.03em;
    }

    .app-header button {
      padding: 8px 20px;
      border-radius: 999px;
      border: 1px solid var(--accent);
      background: var(--accent-soft);
      color: var(--accent);
      font-weight: 600;
      cursor: pointer;
      backdrop-filter: blur(10px);
      transition: all 0.15s ease-out;
    }

    .app-header button:hover {
      background: var(--accent);
      color: #022c22;
      transform: translateY(-1px);
      box-shadow: 0 8px 25px rgba(34, 197, 94, 0.35);
    }

    .layout {
      display: grid;
      grid-template-columns: minmax(0, 2fr) minmax(0, 1.6fr);
      gap: 20px;
    }

    .panel {
      background: var(--bg-panel);
      border-radius: 18px;
      border: 1px solid var(--border);
      padding: 18px 18px 14px;
      box-shadow:
        0 20px 40px rgba(15, 23, 42, 0.7),
        0 0 0 1px rgba(15, 23, 42, 0.8) inset;
      backdrop-filter: blur(20px);
    }

    .panel h2 {
      margin-top: 0;
      font-size: 1.2rem;
    }

    #songs-list {
      list-style: none;
      padding: 0;
      margin: 10px 0 0 0;
      max-height: 420px;
      overflow-y: auto;
    }

    .song-item {
      padding: 8px 10px;
      border-radius: 10px;
      cursor: pointer;
      margin-bottom: 6px;
      display: flex;
      flex-direction: column;
      background: transparent;
      border: 1px solid transparent;
      transition: all 0.12s ease-out;
    }

    .song-item:hover {
      background: rgba(30, 64, 175, 0.28);
      border-color: rgba(59, 130, 246, 0.55);
      transform: translateY(-1px);
    }

    .song-title {
      font-weight: 600;
      font-size: 0.96rem;
    }

    .song-meta {
      font-size: 0.78rem;
      color: var(--text-muted);
      margin-top: 2px;
    }

    .song-item.active {
      background: linear-gradient(135deg, #1d4ed8, #22c55e);
      border-color: transparent;
      box-shadow: 0 10px 30px rgba(59, 130, 246, 0.55);
    }

    .song-item.active .song-title,
    .song-item.active .song-meta {
      color: #f9fafb;
    }

    #audio-player {
      width: 100%;
      margin-top: 18px;
      filter: drop-shadow(0 10px 25px rgba(15, 23, 42, 0.8));
    }

    #now-playing {
      margin-top: 6px;
      font-size: 0.9rem;
      color: var(--text-muted);
    }

    #songs-list::-webkit-scrollbar {
      width: 6px;
    }
    #songs-list::-webkit-scrollbar-track {
      background: transparent;
    }
    #songs-list::-webkit-scrollbar-thumb {
      background: rgba(148, 163, 184, 0.5);
      border-radius: 999px;
    }
  </style>
</head>
<body>
  <div class=""app"">
    <header class=""app-header"">
      <h1>My Music Player</h1>
      <button id=""refresh-btn"">Refresh</button>
    </header>

    <main class=""layout"">
      <section class=""panel"">
        <h2>Songs</h2>
        <ul id=""songs-list""></ul>
      </section>

      <section class=""panel"">
        <h2>Now Playing</h2>
        <div id=""now-playing"">Select a song</div>
        <audio id=""audio-player"" controls preload=""metadata""></audio>
      </section>
    </main>
  </div>

  <script>
    const songsList = document.getElementById(""songs-list"");
    const nowPlaying = document.getElementById(""now-playing"");
    const audioPlayer = document.getElementById(""audio-player"");
    const refreshBtn = document.getElementById(""refresh-btn"");

    async function loadSongs() {
      songsList.innerHTML = ""<li>Loading...</li>"";

      try {
        const res = await fetch(""/api/LocalSongs"");

        if (!res.ok) {
          const text = await res.text();
          console.error(""LocalSongs API error:"", res.status, text);
          songsList.innerHTML = ""<li>Server error loading songs.</li>"";
          return;
        }

        let songs;
        try {
          songs = await res.json();
        } catch (jsonErr) {
          console.error(""Failed to parse JSON from LocalSongs:"", jsonErr);
          songsList.innerHTML = ""<li>Invalid response from server.</li>"";
          return;
        }

        if (!Array.isArray(songs) || songs.length === 0) {
          songsList.innerHTML = ""<li>No songs found. Copy some .mp3 files into wwwroot/music.</li>"";
          return;
        }

        songsList.innerHTML = """"; // clear

        songs.forEach((song) => {
          const li = document.createElement(""li"");
          li.className = ""song-item"";

          const title = document.createElement(""div"");
          title.className = ""song-title"";
          title.textContent = song.title;

          const meta = document.createElement(""div"");
          meta.className = ""song-meta"";
          meta.textContent = ""Local file"";

          li.appendChild(title);
          li.appendChild(meta);

          li.addEventListener(""click"", async () => {
            document
              .querySelectorAll("".song-item"")
              .forEach((el) => el.classList.remove(""active""));
            li.classList.add(""active"");

            nowPlaying.textContent = ""Loading: "" + song.title;
            console.log(""Loading song:"", song.url);
            
            // Stop any currently playing audio
            audioPlayer.pause();
            audioPlayer.currentTime = 0;
            
            // Set up error handler before loading
            let errorHandler = function(e) {
              console.error(""Audio error:"", audioPlayer.error);
              const errorCode = audioPlayer.error ? audioPlayer.error.code : -1;
              let errorMsg = ""Unknown error"";
              if (audioPlayer.error) {
                switch(audioPlayer.error.code) {
                  case 1: errorMsg = ""Media aborted""; break;
                  case 2: errorMsg = ""Network error - file not found""; break;
                  case 3: errorMsg = ""Decode error - invalid audio format""; break;
                  case 4: errorMsg = ""Source not supported""; break;
                  default: errorMsg = audioPlayer.error.message || ""Unknown error"";
                }
              }
              nowPlaying.textContent = song.title + "" - Error: "" + errorMsg;
              console.error(""Audio loading failed. URL:"", song.url, ""Error:"", errorMsg);
            };
            
            audioPlayer.onerror = errorHandler;
            
            // Wait for audio to load before playing
            await new Promise((resolve) => {
              const handleCanPlay = () => {
                audioPlayer.removeEventListener(""canplay"", handleCanPlay);
                audioPlayer.removeEventListener(""error"", errorHandler);
                console.log(""Audio ready to play:"", song.url);
                resolve();
              };
              
              // If already loaded, resolve immediately
              if (audioPlayer.readyState >= 2) {
                resolve();
                return;
              }
              
              audioPlayer.addEventListener(""canplay"", handleCanPlay);
              
              // Set the source and load
              audioPlayer.src = song.url;
              audioPlayer.load();
              
              // Timeout after 10 seconds
              setTimeout(() => {
                if (audioPlayer.readyState < 2) {
                  audioPlayer.removeEventListener(""canplay"", handleCanPlay);
                  audioPlayer.removeEventListener(""error"", errorHandler);
                  console.error(""Audio loading timeout for:"", song.url);
                  nowPlaying.textContent = song.title + "" - Error: Loading timeout. Check if file exists."";
                  resolve();
                }
              }, 10000);
            });
            
            // Try to play
            try {
              nowPlaying.textContent = song.title;
              await audioPlayer.play();
              console.log(""Audio playing successfully:"", song.url);
              audioPlayer.onerror = null;
            } catch (error) {
              console.error(""Error playing audio:"", error);
              nowPlaying.textContent = song.title + "" - Error: Could not play. "" + 
                (error.message || ""Check console for details."");
            }
          });

          songsList.appendChild(li);
        });
      } catch (err) {
        console.error(""Network error calling /api/LocalSongs:"", err);
        songsList.innerHTML = ""<li>Error loading songs.</li>"";
      }
    }

    refreshBtn.addEventListener(""click"", loadSongs);
    loadSongs();
  </script>
</body>
</html>";

        return Content(html, "text/html");
    }
}
