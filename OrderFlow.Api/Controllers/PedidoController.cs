using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Handler;
using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Application.Handler.Pedido;

[ApiController]
[Route("api")]
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
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _getPedidoHandler.HandleAll();
        if (pedidos == null) return NotFound();

        return Ok(pedidos);
    }

    [HttpGet("getByNumber{numeroPedido}")]
    public async Task<IActionResult> GetByNumber(int numeroPedido)
    {
        var pedido = await _getPedidoHandler.HandleByNumber(numeroPedido);
        if (pedido == null) return NotFound();

        return Ok(pedido);
    }
}
