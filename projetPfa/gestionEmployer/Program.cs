using gestionEmployer.Core.Interfaces;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using gestionEmployer.Infrastructure.Data.Repositories;
using gestionEmployer.Core.Services;
using gestionEmployer.Infrastructure.ExternServices;
using Shared.Messaging.Services;
using gestionEmployer.Core.Interfaces.CasheInterfaces;
using StackExchange.Redis;
using Prometheus;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder interfaces services and repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPasswordServiceAdmin, PasswordServiceAdmin>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddMemoryCache();

var redisConnection = builder.Configuration.GetValue<string>("RedisConnection"); // Get Redis connection string from appsettings
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
