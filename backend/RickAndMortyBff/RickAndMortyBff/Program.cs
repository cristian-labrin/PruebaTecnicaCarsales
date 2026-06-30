using RickAndMortyBff.Configuration;
using RickAndMortyBff.Infrastructure;
using RickAndMortyBff.Middleware;
using RickAndMortyBff.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RickAndMortyApiOptions>(
    builder.Configuration.GetSection(RickAndMortyApiOptions.SectionName));

builder.Services.AddHttpClient<IRickAndMortyClient, RickAndMortyClient>((serviceProvider, client) =>
{
    var options = builder.Configuration
        .GetSection(RickAndMortyApiOptions.SectionName)
        .Get<RickAndMortyApiOptions>();

    client.BaseAddress = new Uri(options!.BaseUrl);
});

builder.Services.AddScoped<IEpisodeService, EpisodeService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();

const string corsPolicy = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();