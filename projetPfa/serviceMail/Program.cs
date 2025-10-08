using Prometheus;
using serviceMail.Interfaces;
using serviceMail.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();



app.UseRouting(); 

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});
app.Run();
