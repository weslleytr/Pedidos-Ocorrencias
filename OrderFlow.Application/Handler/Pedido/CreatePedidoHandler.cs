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

            // 2. Adiciona ocorrências via método de domínio (aplica regras)
            foreach (var o in dto.Ocorrencias)
            {
                var ocorrencia = new OrderFlow.Domain.Entities.Ocorrencia(o.TipoOcorrencia);
                pedido.AdicionarOcorrencia(ocorrencia);
            }

            // 3. Persiste pedido via repositório
            _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            // 4. Mapeia para DTO de resposta
            var pedidoDto = new PedidoDto(
                pedido.IdPedido,
                pedido.NumeroPedido,
                pedido.Ocorrencias.Select(o => new OcorrenciaDto(
                    o.IdOcorrencia,
                    o.TipoOcorrencia,
                    o.IndFinalizadora,
                    o.HoraOcorrencia
                )).ToList(),
                pedido.IndEntregue,
                pedido.HoraPedido
            );

            return pedidoDto;
        }
    }
}
