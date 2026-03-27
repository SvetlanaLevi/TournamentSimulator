using TournamentSimulator.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IMatchSimulator, MatchSimulator>();
builder.Services.AddScoped<IMatchScheduler, FixedMatchScheduler>();
builder.Services.AddScoped<ITeamProvider, StaticTeamProvider>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseCors();

app.MapControllers();

app.Run();
