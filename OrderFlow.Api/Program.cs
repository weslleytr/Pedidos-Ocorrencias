using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infra.Data;
using OrderFlow.Infra.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("OrderFlow.Infra") 
    )
);

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<CreatePedidoHandler>();
builder.Services.AddScoped<GetPedidoHandler>();
builder.Services.AddScoped<IOcorrenciaRepository, OcorrenciaRepository>();
builder.Services.AddScoped<CreateOcorrenciaHandler>();
builder.Services.AddScoped<DeleteOcorrenciaHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
