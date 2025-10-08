using Auth;
using Auth.Interfaces;
using Auth.Model;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IValidateTokenBuilder, ValidateTokenBuilder>(); // Register builder as Scoped or Transient
builder.Services.AddScoped<ValidateTokenDirector>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<UserService>();
builder.Services.AddHttpClient<EmployerService>();
builder.Services.AddHttpClient<AdminService>();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployerService, EmployerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IFacebookAuthService, FacebookAuthService>();
builder.Services.AddScoped<FacebookLoginAdapter>();
// Registering services for Google Auth, User Service, and Login Service
builder.Services.AddScoped<IUserService, UserService>(); // Your existing UserService

builder.Services.AddScoped<Func<string, ILoginService>>(serviceProvider => key =>
{
    return key switch
    {
        "facebook" => serviceProvider.GetRequiredService<FacebookLoginAdapter>(),
        "google" => serviceProvider.GetRequiredService<GoogleLoginAdapter>(),
        _ => serviceProvider.GetRequiredService<UserService>(), // Default login method
    };
});
builder.Services.AddScoped<GoogleLoginAdapter>();

builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
// JWT Authentication setup (this part may already be in your `Program.cs` file)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001"; // Replace with your JWT Authority URL
        options.Audience = "YourAudience";  // Replace with your JWT Audience
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILoginService, FacebookLoginAdapter>();  // For Facebook login
builder.Services.AddScoped<ILoginService, UserService>();  // Default login (normal)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting(); 

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});
app.Run();
