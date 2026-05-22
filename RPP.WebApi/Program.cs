using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RPP.BusinessLogicsContracts;
using RPP.Common.Infrastructure;
using RPP.Database;
using RPP.Database.DatabaseImplementations;
using RPP.Database.Mappings;
using RPP.DatabaseImplementations;
using RPP.Implementations;
using RPP.StoragesContracts;
using RPP.WebApi.Adapters;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Mappings;
using RPP.WebApi.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========== НАСТРОЙКА ЛОГГИРОВАНИЯ ==========
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// ========== НАСТРОЙКА АУТЕНТИФИКАЦИИ ==========
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "RPP_Server",
            ValidateAudience = true,
            ValidAudience = "RPP_Client",
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RPP_SuperSecretKey_1234567890_SecretKey_1234567890")),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

// ========== НАСТРОЙКА SWAGGER ==========
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RPP API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

// ========== НАСТРОЙКА БАЗЫ ДАННЫХ ==========
builder.Services.AddDbContext<CatHasPawsDbContext>(options =>
    options.UseInMemoryDatabase("RPP_Database"));

// ========== РЕГИСТРАЦИЯ ЗАВИСИМОСТЕЙ ==========

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DataModelMappingProfile>();
    cfg.AddProfile<ApiMappingProfile>();
});

// Storages
builder.Services.AddScoped<IClientStorageContract, ClientStorageContract>();
builder.Services.AddScoped<IWorkerStorageContract, WorkerStorageContract>();
builder.Services.AddScoped<IHomeStorageContract, HomeStorageContract>();
builder.Services.AddScoped<IToolStorageContract, ToolStorageContract>();
builder.Services.AddScoped<IWorkTypeStorageContract, WorkTypeStorageContract>();
builder.Services.AddScoped<IReportStorageContract, ReportStorageContract>();
builder.Services.AddScoped<IPostStorageContract, PostStorageContract>();
builder.Services.AddScoped<IBuyerStorageContract, BuyerStorageContract>();
builder.Services.AddScoped<IProductStorageContract, ProductStorageContract>();
builder.Services.AddScoped<IManufacturerStorageContract, ManufacturerStorageContract>();

// Business Logic
builder.Services.AddScoped<IClientBusinessLogicContract, ClientBusinessLogicContract>();
builder.Services.AddScoped<IWorkerBusinessLogicContract, WorkerBusinessLogicContract>();
builder.Services.AddScoped<IHomeBusinessLogicContract, HomeBusinessLogicContract>();
builder.Services.AddScoped<IToolBusinessLogicContract, ToolBusinessLogicContract>();
builder.Services.AddScoped<IWorkTypeBusinessLogicContract, WorkTypeBusinessLogicContract>();
builder.Services.AddScoped<IReportBusinessLogicContract, ReportBusinessLogicContract>();
builder.Services.AddScoped<IPostBusinessLogicContract, PostBusinessLogicContract>();

// Adapters
builder.Services.AddScoped<ClientAdapter>();
builder.Services.AddScoped<WorkerAdapter>();
builder.Services.AddScoped<HomeAdapter>();
builder.Services.AddScoped<ToolAdapter>();
builder.Services.AddScoped<WorkTypeAdapter>();
builder.Services.AddScoped<ReportAdapter>();
builder.Services.AddScoped<PostAdapter>();

// ========== НАСТРОЙКИ ДЛЯ 5 ЛАБЫ ==========
builder.Services.AddSingleton<IConfigurationSalary, ConfigurationSalary>();

// ========== НАСТРОЙКИ ДЛЯ 6 ЛАБЫ ==========
builder.Services.AddScoped<ReportService>();

var app = builder.Build();

// ========== НАСТРОЙКА PIPELINE ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ========== ТОКЕН ==========
app.MapGet("/login", () =>
{
    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes("RPP_SuperSecretKey_1234567890_SecretKey_1234567890");
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "user")
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = "RPP_Server",
        Audience = "RPP_Client",
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
});

app.Run();

public partial class Program { }