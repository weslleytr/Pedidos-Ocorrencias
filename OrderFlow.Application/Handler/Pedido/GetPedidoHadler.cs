using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Handler.Pedido
{
    public class GetPedidoHandler
    {
        private readonly IPedidoRepository _pedidoRepository;

        public GetPedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDto?> HandleByNumber(int numeroPedido)
        {
            // Buscar o pedido no repositório
            var pedido = await _pedidoRepository.GetPedidoByNumberAsync(numeroPedido);
            if (pedido == null) return null; // Retorna null se não existir

            // Mapear entidade para DTO
            var dto = new PedidoDto(
                pedido.IdPedido,
                pedido.NumeroPedido,
                pedido.IndEntregue,
                pedido.HoraPedido,
                pedido.Ocorrencias.Select(o => new OcorrenciaDto(
                    o.IdOcorrencia,
                    o.TipoOcorrencia,
                    o.IndFinalizadora,
                    o.HoraOcorrencia,
                    o.PedidoId
                )).ToList()
            );

            return dto;
        }

        public async Task<List<PedidoDto>> HandleAll()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            if (pedidos == null || !pedidos.Any()) return new List<PedidoDto>();

            return pedidos.Select(p => new PedidoDto(
                p.IdPedido,
                p.NumeroPedido,
                p.IndEntregue,
                p.HoraPedido,
                p.Ocorrencias.Select(o => new OcorrenciaDto(
                    o.IdOcorrencia,
                    o.TipoOcorrencia,
                    o.IndFinalizadora,
                    o.HoraOcorrencia,
                    o.PedidoId
                )).ToList()
            )).ToList();
        }
    }
}
