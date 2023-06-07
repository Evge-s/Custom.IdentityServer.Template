using IdentityServer.Api.Middlewares;
using IdentityServer.Api.Models.ServiceData;
using IdentityServer.Services.AuthService;
using IdentityServer.Services.JwtService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ServiceContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

builder.Services.AddDbContext<ServiceContext>(options =>
{
    var server = builder.Configuration.GetValue<string>("ConnectionStrings:Server");
    var database = builder.Configuration.GetValue<string>("ConnectionStrings:Database");
    var user = builder.Configuration.GetValue<string>("ConnectionStrings:User");

    // Read password from Docker Secret
    string passwordFilePath = builder.Configuration.GetValue<string>("ConnectionStrings:PasswordFilePath");
    string password = File.ReadAllText(passwordFilePath);

    string connectionString = $"Server={server};Database={database};User={user};Password={password};TrustServerCertificate=true";

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

/*
// Add DbContext
string cnnName = "DefaultDbConnection";
var str = builder.Configuration.GetConnectionString($"ConnectionStrings:{cnnName}");
builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(str));
*/

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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