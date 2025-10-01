using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infra.Data;
using OrderFlow.Infra.Repositories;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------
// Configuração do Serilog para logs
// --------------------------------
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// --------------------------------
// Configuração do DbContext com PostgreSQL
// --------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("OrderFlow.Infra") // Define a assembly de migrations
    )
);

// --------------------------------
// Habilita Serilog como provedor de logs da aplicação
// --------------------------------
builder.Host.UseSerilog();

// --------------------------------
// Registro de Repositórios e Handlers via DI
// --------------------------------
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<CreatePedidoHandler>();
builder.Services.AddScoped<GetPedidoHandler>();
builder.Services.AddScoped<IOcorrenciaRepository, OcorrenciaRepository>();
builder.Services.AddScoped<CreateOcorrenciaHandler>();
builder.Services.AddScoped<DeleteOcorrenciaHandler>();

// --------------------------------
// Configuração do JWT Authentication
// --------------------------------
var key = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:KeyJwt"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(c =>
{
    c.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

// --------------------------------
// Configuração do MVC e Swagger
// --------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // --------------------------------
    // Informações da API
    // --------------------------------
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrderFlow API",
        Version = "v1",
        Description = "API para gerenciamento de pedidos e ocorrências"
    });

    // --------------------------------
    // Habilita anotações via Swagger
    // --------------------------------
    c.EnableAnnotations();

    // --------------------------------
    // Configuração do esquema de segurança JWT
    // --------------------------------
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta forma: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

var app = builder.Build();

// --------------------------------
// Configuração do Swagger no ambiente de desenvolvimento
// --------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderFlow API v1");
        c.RoutePrefix = string.Empty;
    });
}

// --------------------------------
// Middleware de HTTPS, Autenticação e Autorização
// --------------------------------
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// --------------------------------
// Mapeamento dos controllers
// --------------------------------
app.MapControllers();

// --------------------------------
// Inicializa a aplicação
// --------------------------------
app.Run();
