using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using Identity.Api.Middlewares;
using Identity.Api.Models.Filters;
using Identity.Api.Models.ServiceData;
using Identity.Api.Services.AuthService;
using Identity.Api.Services.DbCleanupService;
using Identity.Api.Services.EmailService;
using Identity.Api.Services.JwtService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServiceContext>(options =>
{
    string connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Configure Services
builder.Services.AddControllers();

builder.Services.Configure<MvcOptions>(options => options.Filters.Add(new ProducesAttribute("application/json")));
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Configure Hangfire to use SQL Server
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
        new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            UsePageLocksOnDequeue = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer", Version = "v1" });
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICleanupService, CleanupService>();

builder.Services.AddSingleton<IEmailService>(new EmailService(
    builder.Configuration.GetValue<string>("EmailServiceData:ApiKey"),
    builder.Configuration.GetValue<string>("EmailServiceData:ServiceUserName")));

builder.Services.AddHangfireServer();

// Configure Middleware
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();


app.UseHangfireServer();
app.UseHangfireDashboard("/hangfire-dashboard", new DashboardOptions
{
    //Authorization = new[] { new HangfireAuthorizationFilter() }
});

// Get the service scope factory, create a jobActivator for Hangfire,
// and set up a regular job to clean up overdue commits every day
var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var jobActivator = new AspNetCoreJobActivator(serviceScopeFactory);
JobActivator.Current = jobActivator;

using (var serviceScope = app.Services.CreateScope())
{
    var cleanupService = serviceScope.ServiceProvider.GetRequiredService<ICleanupService>();
    RecurringJob.AddOrUpdate(() => cleanupService.CleanUpExpiredConfirmations(), Cron.Daily);
}

app.Run();