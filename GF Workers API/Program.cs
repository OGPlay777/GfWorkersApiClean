using GF_Workers_API.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using OperationWorker.Application.Services;
using OperationWorker.Core.Abstractions;
using OperationWorker.Core.Abstractions.Auth;
using OperationWorker.Core.Abstractions.Notificators;
using OperationWorker.Core.Abstractions.Repos;
using OperationWorker.DataAccess;
using OperationWorker.DataAccess.Repositories;
using OperationWorker.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

var configuration = builder.Configuration;
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(configuration)
);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OperationWorkerDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddScoped<IAppUsersService, AppUsersService>();
builder.Services.AddScoped<IAppUsersRepository, AppUsersRepository>();

builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

builder.Services.AddScoped<IGfWorkersService, GfWorkersService>();
builder.Services.AddScoped<IGfWorkersRepository, GfWorkersRepository>();

builder.Services.AddScoped<IOperationsService, OperationsService>();
builder.Services.AddScoped<IOperationsRepository, OperationsRepository>();

builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();

builder.Services.AddScoped<IPaintPowderService, PaintPowderService>();
builder.Services.AddScoped<IPaintPowderRepository, PaintPowderRepository>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IIdentityProvider, IdentityProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<INotificationSender, NotificationSender>();


builder.Services.AddApiAuthentication(configuration);
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();
