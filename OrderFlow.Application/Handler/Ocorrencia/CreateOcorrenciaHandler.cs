using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Handler.Ocorrencia
{
    public class CreateOcorrenciaHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public CreateOcorrenciaHandler(
            IPedidoRepository pedidoRepository,
            IOcorrenciaRepository ocorrenciaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        public async Task<OcorrenciaDto> Handle(CreateOcorrenciaDto dto)
        {
            // 1. Carrega o pedido relacionado do banco
            var pedido = await _pedidoRepository.GetPedidoByNumberAsync(dto.NumeroPedido);
            if (pedido == null)
                throw new InvalidOperationException("Pedido não encontrado.");

            // 2. Cria a nova ocorrência
            var ocorrencia = new OrderFlow.Domain.Entities.Ocorrencia(dto.TipoOcorrencia)
            {
                PedidoId = pedido.IdPedido
            };

            // 3. Usa a regra de domínio do Pedido existente
            pedido.AdicionarOcorrencia(ocorrencia);

            // 4. Persiste a ocorrência
            await _ocorrenciaRepository.AddAsync(ocorrencia);

            // 5. Atualiza o pedido no banco (IndEntregue pode ter mudado)
            _pedidoRepository.UpdateAsync(pedido);

            // 6. Salva tudo no banco
            await _ocorrenciaRepository.SaveChangesAsync();
            await _pedidoRepository.SaveChangesAsync();

            // 7. Retorna DTO
            return new OcorrenciaDto(
                ocorrencia.IdOcorrencia,
                ocorrencia.TipoOcorrencia,
                ocorrencia.IndFinalizadora,
                ocorrencia.HoraOcorrencia,
                ocorrencia.PedidoId
            );
        }

    }
}
