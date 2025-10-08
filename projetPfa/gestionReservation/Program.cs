
using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.Data.Repositories;
using gestionReservation.Infrastructure.ExternServices;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration du DbContext (acc�s � la base de donn�es)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Ajouter les services n�cessaires � l'injection de d�pendances
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();  // Repository pour Reservation
builder.Services.AddScoped<IReservationService, ReservationService>();  // Service pour Reservation
builder.Services.AddScoped<IStatusService, StatusService>();  // Service pour Status (si besoin)
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

builder.Services.AddScoped<ISiteService,SiteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMailService, MailService>();


// Si vous utilisez des mappers ou des services suppl�mentaires, vous pouvez les ajouter ici aussi
// builder.Services.AddScoped<IReservationMapper, ReservationMapper>();

// Ajouter les contr�leurs pour exposer les API
builder.Services.AddControllers();

//http client
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();



app.UseRouting(); 

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});
// D�marrer l'application
app.Run();
