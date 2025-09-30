using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Handler.Pedido
{
    public class CreatePedidoHandler
    {
        private readonly IPedidoRepository _pedidoRepository;

        public CreatePedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDto> Handle(CreatePedidoDto dto)
        {
            // 1. Cria pedido no domínio
            var pedido = new OrderFlow.Domain.Entities.Pedido(dto.NumeroPedido);

            if (await _pedidoRepository.GetPedidoByNumberAsync(pedido.NumeroPedido))
                throw new InvalidOperationException("Já existe um pedido com este número.");
            // 3. Persiste pedido via repositório
            _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            // 4. Mapeia para DTO de resposta
            var pedidoDto = new PedidoDto(
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

            return pedidoDto;
        }
    }
}
