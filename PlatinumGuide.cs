using NSwag.AspNetCore;
using Plats.Data;
using Plats.Models;

class HelloWeb
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "PlatinumAPI";
            config.Title = "PlatinumAPI v1";
            config.Version = "v1";
        });

        builder.Services.AddDbContext<AppDbContext>();
        WebApplication app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi(config =>
            {
                config.DocumentTitle = "Platinum API";
                config.Path = "/swagger";
                config.DocumentPath = "/swagger/{documentName}/swagger.json";
                config.DocExpansion = "list";
            });
        }

        app.MapGet("/games", (AppDbContext schema) => {
            var plats = schema.Plats;
            if(plats.Any()){
                return Results.Ok(plats);
            }else{
                return Results.BadRequest("Sem dados para exibir.");
            }
        }).Produces<Platinum>();

        app.MapGet("/games/{id}", (AppDbContext schema, Guid id) => {
            var game = schema.Plats.FirstOrDefault(g => g.Id == id);
            if(game != null){
                return Results.Ok(game);
            }else{
                return Results.NotFound("Jogo não encontrado.");
            }
        }).Produces<Platinum>();

        app.MapGet("/games/difficulty/{diff}", (AppDbContext schema, int diff) => {
            var difficulty = schema.Plats.Where(p => p.Difficulty == diff);

            if(diff >= 0 && diff <= 10){
                if(difficulty.Any()){
                    return Results.Ok(difficulty);
                }else{
                    return Results.BadRequest("Jogo na dificuldade " + diff.ToString() + " não localizado.");
                }
            }else{
                    return Results.NotFound();
                }   
        }).Produces<Platinum>();

        app.MapPost("/games", (AppDbContext schema, Platinum plat) => {
            schema.Plats.Add(plat);
            schema.SaveChanges();
            return Results.Created($"/games/{plat.Id}", plat);
        }).Produces<Platinum>();

        app.MapPut("/games/{id}", (AppDbContext schema, Guid id, Platinum updatedPlat) => {
            var plat = schema.Plats.Find(id);
            if (plat == null) {
                return Results.BadRequest($"Jogo {id} não foi localizado.");
            }
            var newPlat = plat with {
                GameName = updatedPlat.GameName,
                TrophyName = updatedPlat.TrophyName,
                Difficulty = updatedPlat.Difficulty,
                AverageTime = updatedPlat.AverageTime
            };

            schema.Entry(plat).CurrentValues.SetValues(newPlat);
            schema.SaveChanges();
            return Results.Ok(newPlat);
        }).Produces<Platinum>();


        app.MapPatch("/games/difficulty", (AppDbContext schema, Guid id, int difficulty) => {
            var plat = schema.Plats.Find(id);
            if (plat == null) {
                return Results.BadRequest($"Jogo {id} não foi localizado.");
            }
            var newPlat = plat with {
                Difficulty = difficulty
            };

            schema.Entry(plat).CurrentValues.SetValues(newPlat);
            schema.SaveChanges();
            return Results.Ok(newPlat);
        }).Produces<Platinum>();

        app.MapDelete("/games/{id}", (AppDbContext schema, Guid id) => {
            var plat = schema.Plats.Find(id);
            if (plat == null) {
                return Results.BadRequest($"Jogo {id} não foi localizado.");
            }
            schema.Plats.Remove(plat);
            schema.SaveChanges();
            return Results.Ok($"Jogo {id} removido com sucesso.");
        }).Produces<Platinum>();    

        
        app.Run();
    }
}
