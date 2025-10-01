using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Controllers;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler;
using OrderFlow.Application.Handler.Pedido;
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
    public async Task<IActionResult> Criar([FromBody] CreatePedidoDto dto)
    {
        try
        {
            var pedido = await _createPedidoHandler.Handle(dto);
            _logger.LogInformation("Pedido criado com sucesso: {NumeroPedido}", dto.NumeroPedido);
            return CreatedAtAction(
                nameof(GetByNumber),
                new { numeroPedido = pedido.NumeroPedido },
                pedido
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pedido: {NumeroPedido}", dto.NumeroPedido);
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("getAll")]
    [SwaggerOperation(Summary = "Lista todos os pedidos", Description = "Retorna todos os pedidos cadastrados.")]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _getPedidoHandler.HandleAll();
        if (pedidos == null) return NotFound();
        _logger.LogInformation("Lista de pedidos retornada com sucesso, total: {Total}", pedidos.Count);
        return Ok(pedidos);
    }

    [HttpGet("getByNumber{numeroPedido}")]
    [SwaggerOperation(Summary = "Busca pedido por número", Description = "Retorna um pedido específico pelo seu número.")]
    public async Task<IActionResult> GetByNumber(int numeroPedido)
    {
        var pedido = await _getPedidoHandler.HandleByNumber(numeroPedido);
        if (pedido == null) return NotFound();
        _logger.LogInformation("Pedido retornado com sucesso: {NumeroPedido}", numeroPedido);
        return Ok(pedido);
    }
}
