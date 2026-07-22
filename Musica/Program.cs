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

 app.MapPost("/melodia", (JsonElement body) =>
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
app.MapGet("/melodia/listagem", () =>
{
    Melodia[] melodiasCadastradas = new Melodia[totalMelodias];
    for(int i = 0; i < totalMelodias; i++)
    {
        melodiasCadastradas[i] = melodias[i];
    }
    return Results.Ok(
    new{ melodiasCadastradas}
    );
 });
 app.MapGet("melodia/{titulo}", (string titulo) =>
{
   Melodia[] melodiasEncontradas = new Melodia[totalMelodias];

    int totalEncontradas = 0;

    for (int i = 0; i < totalMelodias; i++)
    {
        if (melodias[i].Titulo.ToLower() == titulo.ToLower())
       
        {
            melodiasEncontradas[totalEncontradas] = melodias[i];
            totalEncontradas++;
        }
    }

    if (totalEncontradas > 0)
    {
        Melodia[] resultadoFinal = new Melodia[totalEncontradas];

        for (int i = 0; i < totalEncontradas; i++)
        {
            resultadoFinal[i] = melodiasEncontradas[i];
        }        

        return Results.Ok(new
        {
            titulo,
            melodias = melodiasEncontradas
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhum titulo como esse encontrado"
    });
});
app.MapPatch("/melodia/{id}", (int id, JsonElement body) =>
{
    string novo_titulo = body.GetProperty("titulo").GetString();

    for (int i = 0; i < totalMelodias; i++)
    {
        if (melodias[i].Id == id)
        {
            melodias[i].Titulo = novo_titulo;

            return Results.Ok(
                new
                {
                    melodia = melodias[i]
                }
            );
        }
    }

    return Results.NotFound(new
    {
        message = "Música não encontrada."
    });
});
app.MapDelete("/melodia", (int id) =>
{
    for (int i = 0; i < totalMelodias; i++)
    {
        if (melodias[i].Id == id)
        {
            Melodia melodiaRemovida = melodias[i];
            
            for (int j = i; j < totalMelodias - 1; j++)
            {
                melodias[j] = melodias[j + 1];
            }            

            totalMelodias--;

            return Results.Ok(new
            {
                mensagem = "Musica removida com sucesso.",
                melodia = melodiaRemovida
            });
        }
    }

    return Results.NotFound(new
    {
        message = "Melodia não encontrada."
    });
});
app.Run();