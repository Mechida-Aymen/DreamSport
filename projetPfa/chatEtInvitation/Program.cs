using chatEtInvitation.API.Extentions;
using chatEtInvitation.Infrastructure.ExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Services;
using chatEtInvitation.Infrastructure.Data.Repositories;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using chatEtInvitation.Core.Interfaces.Hub;
using Prometheus;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder interfaces services and repositories
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IMemberInvitationRepository, MemberInvitationRepository>();
builder.Services.AddScoped<IchatAmisService,AmisChatService>();
builder.Services.AddScoped<IchatTeamService, TeamChatService>();
builder.Services.AddScoped<ITeamMessageRepository,TeamMessageRepository>();
builder.Services.AddScoped<IChatAmisMessageRepository, ChatAmisMessageRepository>();
builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddScoped<IBloqueListRepository, BloqueListRepository>();


builder.Services.AddScoped<TeamChatConsumerService>();
builder.Services.AddSingleton<IHostedService, TeamChatConsumerService>();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<UserService>();
// In Program.cs or Startup.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.addRepositoriesDependencies();
builder.Services.addServicesDependencies();

builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

builder.Services.AddSignalR()
    .AddJsonProtocol(options => {
        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    });
var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseRouting(); 
app.UseCors("AllowAll");

app.MapHub<InvitationHub>("/invitationHub");

app.MapHub<ChatHub>("/chatHub");

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});
app.Run();