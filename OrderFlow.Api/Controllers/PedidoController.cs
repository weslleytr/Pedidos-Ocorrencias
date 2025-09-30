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
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoController(CreatePedidoHandler createPedidoHandler, IPedidoRepository pedidoRepository)
    {
        _createPedidoHandler = createPedidoHandler;
        _pedidoRepository = pedidoRepository;
    }

    [HttpPost("create-pedido")]
    public async Task<IActionResult> Criar([FromBody] CreatePedidoDto dto)
    {
        try
        {
            var pedido = await _createPedidoHandler.Handle(dto);
            return CreatedAtAction(nameof(GetById), new { id = pedido.IdPedido }, pedido);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("getAllPedidos")]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _pedidoRepository.GetAllAsync();

        var dtos = pedidos.Select(p => new PedidoDto(
            p.IdPedido,
            p.NumeroPedido,
            p.IndEntregue,
            p.HoraPedido,
            p.Ocorrencias.Select(o => new OcorrenciaDto(
                o.IdOcorrencia,
                o.TipoOcorrencia,
                o.IndFinalizadora,
                o.HoraOcorrencia
            )).ToList()
        )).ToList();

        return Ok(dtos);
    }

    [HttpGet("getPedidoById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(id);
        if (pedido == null) return NotFound();

        var dto = new PedidoDto(
            pedido.IdPedido,
            pedido.NumeroPedido,
            pedido.IndEntregue,
            pedido.HoraPedido,
            pedido.Ocorrencias.Select(o => new OcorrenciaDto(
                o.IdOcorrencia,
                o.TipoOcorrencia,
                o.IndFinalizadora,
                o.HoraOcorrencia
            )).ToList()
        );

        return Ok(dto);
    }
}
