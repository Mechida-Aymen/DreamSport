
using System.Diagnostics;
using System.Security.Cryptography;
using gestionSite.Core.Interfaces.AnnoncesInterfaces;
using gestionSite.Core.Interfaces.CasheInterfaces;
using gestionSite.Core.Interfaces.SiteInterfaces;
using gestionSite.Core.Interfaces.SportInterfaces;
using gestionSite.Core.Interfaces.TerrainInterfaces;
using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;
using gestionSite.Core.Services;
using gestionSite.Infrastructure.Data.Repositories;
using gestionSite.Infrastructure.Repositories;
using Prometheus;
using Shared.Messaging.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Ajoutez ceci dans votre ConfigureServices
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);



builder.Services.AddControllers();

builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddScoped<ISportService, SportService>();
builder.Services.AddScoped<ISiteService, SiteService>();

builder.Services.AddScoped<ITerrainStatusRepository, TerrainStatusRepository>();
builder.Services.AddScoped<ITerrainStatusService, TerrainStatusService>();


builder.Services.AddSiteDependencies();
builder.Services.addAnnoncesDependencies ();
builder.Services.AddFaqDependencies();
builder.Services.AddTerrainDependencies();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<SiteConsumerService>();

// Ajoutez RabbitMQConsumerService pour SiteConsumerService
builder.Services.AddSingleton<IHostedService, SiteConsumerService>();

builder.Services.AddMemoryCache();

var redisConnection = builder.Configuration.GetValue<string>("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<ICacheService, LazyCacheDecorator>();

builder.Services.AddSingleton<InMemoryCacheService>();
builder.Services.AddSingleton<RedisCacheService>();

builder.Services.AddSingleton<ICacheService>(sp =>
{
    var memory = sp.GetRequiredService<InMemoryCacheService>();
    var redis = sp.GetRequiredService<RedisCacheService>();
    return new LazyCacheDecorator(memory, redis);
});
var app = builder.Build();


app.UseRouting(); 


app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});
app.Run();

