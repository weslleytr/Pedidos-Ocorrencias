using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    private readonly CreatePedidoHandler _createPedidoHandler;
    private readonly GetPedidoHandler _getPedidoHandler;

    public PedidoController(CreatePedidoHandler createPedidoHandler, GetPedidoHandler getPedidoHandler)
    {
        _createPedidoHandler = createPedidoHandler;
        _getPedidoHandler = getPedidoHandler;
    }

    [HttpPost("create-pedido")]
    [SwaggerOperation(Summary = "Cria um novo pedido", Description = "Cria um pedido com os dados fornecidos e retorna o pedido criado.")]
    public async Task<IActionResult> Criar([FromBody] CreatePedidoDto dto)
    {
        try
        {
            var pedido = await _createPedidoHandler.Handle(dto);
            return CreatedAtAction(
                nameof(GetByNumber),
                new { numeroPedido = pedido.NumeroPedido },
                pedido
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("getAll")]
    [SwaggerOperation(Summary = "Lista todos os pedidos", Description = "Retorna todos os pedidos cadastrados.")]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _getPedidoHandler.HandleAll();
        if (pedidos == null) return NotFound();

        return Ok(pedidos);
    }

    [HttpGet("getByNumber{numeroPedido}")]
    [SwaggerOperation(Summary = "Busca pedido por número", Description = "Retorna um pedido específico pelo seu número.")]
    public async Task<IActionResult> GetByNumber(int numeroPedido)
    {
        var pedido = await _getPedidoHandler.HandleByNumber(numeroPedido);
        if (pedido == null) return NotFound();

        return Ok(pedido);
    }
}
