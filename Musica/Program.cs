using Musica.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

app.UseCors("AllowAll");

Melodia[] melodias = new Melodia[100];
int totalMelodias = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API Musica funcionando com sucesso!");
});

 app.MapPost("/Melodia", (JsonElement body) =>
{
    Random random = new();
        Melodia melodia = new Melodia();

        melodia.Id = random.Next(1000,9999);
        melodia.Titulo = body.GetProperty("titulo").GetString();
        melodia.Artista = body.GetProperty("artista").GetString();
        melodia.Compositor = body.GetProperty("compositor").GetString();
        melodia.Genero = body.GetProperty("genero").GetString();
        melodia.Ano = body.GetProperty("ano").GetInt32();
        melodias[totalMelodias] = melodia;
        totalMelodias++;
        return Results.Ok(

            new {melodia}
        );
});

app.Run();