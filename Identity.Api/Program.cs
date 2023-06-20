using Identity.Api.Middlewares;
using Identity.Api.Models.ServiceData;
using Identity.Api.Services.AuthService;
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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer", Version = "v1" });
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IEmailService>(new EmailService(
    builder.Configuration.GetValue<string>("EmailServiceData:Domain"),
    builder.Configuration.GetValue<int>("EmailServiceData:Port"),
    builder.Configuration.GetValue<string>("EmailServiceData:UserName"),
    builder.Configuration.GetValue<string>("EmailServiceData:Password")));

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

app.Run();