using AutoMapper;
using DotNetEnv;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyLibrary.Server.Configs;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.EventHandlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

config.AddUserSecrets<Program>();


// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = JwtConfig.JwtIssuer,
            ValidAudience = JwtConfig.JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.DescribeAllParametersInCamelCase();
    options.AddServer(new OpenApiServer
    {
        Description = "Development Server",
        Url = "https://localhost:7179"

    });
    options.CustomOperationIds(e =>
    {
        return e.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

})
    .AddSwaggerGenNewtonsoftSupport();

builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Library API";
    config.Version = "v1";
    config.Description = "API Documentation for the Library System.";

    config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Enter JWT token in this format: Bearer {your_token}"
    });

    config.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.UseSqlServer(DbConfig.ConnectionString, builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Custom services
builder.Services.AddSingleton<EventBus>();
builder.Services.AddScoped<IOperationsEventHandler, OperationsEventHandler>();
builder.Services.AddLogging();
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapHandler>();
});
builder.Services.AddScoped<IBookHandler<IBook<Guid>>, BookHandler>();
builder.Services.AddScoped<IWarehouseHandler<IWarehouse<int>>, WarehouseHandler>();
builder.Services.AddScoped<IOperationHandler, OperationHandler>();
builder.Services.AddScoped<IAuthHandler, AuthHandler>();
builder.Services.AddScoped<IUserHandler, UserHandler>();
builder.Services.AddScoped<ISubscriptionHandler<User>, SubscriptionHandler>();
builder.Services.AddScoped<IResultHandler<ITaskResult>, ResultHandler>();
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();

// Health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(DbConfig.ConnectionString, name: "SQL Server", tags: new[] { "db" });

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(15);
    options.MaximumHistoryEntriesPerEndpoint(60);
    options.AddHealthCheckEndpoint("Health Check", "/health");
})
    .AddSqlServerStorage(DbConfig.ConnectionString);


builder.Host.UseSerilog();
var app = builder.Build();


app.UseCors(builder => builder
.WithOrigins("*")
.AllowAnyMethod()
.AllowAnyHeader()
);

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Resources", "Images")),
    RequestPath = "/images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-ui-api";
    options.ResourcesPath = "/health-ui/resources";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
