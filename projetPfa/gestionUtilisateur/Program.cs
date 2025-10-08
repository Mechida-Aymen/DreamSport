using gestionUtilisateur.API.Extensions;
using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Services;
using gestionUtilisateur.Infrastructure.Data;
using gestionUtilisateur.Infrastructure.Data.Repositories;
using gestionUtilisateur.Infrastructure.Extern_Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IMailService,MailService>();
builder.Services.AddScoped<IPasswordUserService, PasswordUserService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IAuthService, AuthService>();



// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();



app.UseRouting(); 

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});
app.Run();
