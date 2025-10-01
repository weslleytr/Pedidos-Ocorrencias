using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Controllers;
using OrderFlow.Api.Core;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api")]
[Authorize]
public class PedidoController : ControllerBase
{
    private readonly ILogger<OcorrenciaController> _logger;
    private readonly CreatePedidoHandler _createPedidoHandler;
    private readonly GetPedidoHandler _getPedidoHandler;

    public PedidoController(CreatePedidoHandler createPedidoHandler, GetPedidoHandler getPedidoHandler, ILogger<OcorrenciaController> logger)
    {
        _createPedidoHandler = createPedidoHandler;
        _getPedidoHandler = getPedidoHandler;
        _logger = logger;
    }

    [HttpPost("create-pedido")]
    [SwaggerOperation(Summary = "Cria um novo pedido", Description = "Cria um pedido com os dados fornecidos e retorna o pedido criado.")]
    public async Task<IResult> Criar([FromBody] CreatePedidoDto dto)
    {
        var pedido = await _createPedidoHandler.Handle(dto);
        return Results.Extensions.MapResults(pedido);
    }

    [HttpGet("getAll")]
    [SwaggerOperation(Summary = "Lista todos os pedidos", Description = "Retorna todos os pedidos cadastrados.")]
    public async Task<IResult> GetAll()
    {
        var pedidos = await _getPedidoHandler.HandleAll();
        return Results.Extensions.MapResults(pedidos);
    }

    [HttpGet("getByNumber{numeroPedido}")]
    [SwaggerOperation(Summary = "Busca pedido por número", Description = "Retorna um pedido específico pelo seu número.")]
    public async Task<IResult> GetByNumber(int numeroPedido)
    {
        var pedido = await _getPedidoHandler.HandleByNumber(numeroPedido);
        return Results.Extensions.MapResults(pedido);
    }
}
