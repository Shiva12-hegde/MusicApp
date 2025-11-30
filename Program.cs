using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();      // ✅ needed for Swagger
builder.Services.AddSwaggerGen();                // ✅ needed for Swagger

builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseInMemoryDatabase("MusicDb"));

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ✅ Enable Swagger for ALL environments
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Enable static files serving (needed for MP3 files in wwwroot/music)
// This must come before routing/controllers so static files are checked first
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// ✅ Root `/` + all API routes are handled by controllers
app.MapControllers();

// ✅ Force listen on http://localhost:5000
app.Run("http://localhost:5000");
