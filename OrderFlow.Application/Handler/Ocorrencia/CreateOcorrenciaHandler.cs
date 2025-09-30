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
            // 1. Carrega o pedido relacionado
            var pedido = await _pedidoRepository.GetByIdAsync(dto.);
            if (pedido == null)
                throw new InvalidOperationException("Pedido não encontrado.");

            // 2. Cria a nova ocorrência
            var ocorrencia = new OrderFlow.Domain.Entities.Ocorrencia(dto.TipoOcorrencia)
            {
                PedidoId = dto.PedidoId
            };

            // 3. Usa a regra de domínio do Pedido
            pedido.AdicionarOcorrencia(ocorrencia);

            // 4. Persiste no banco
            _ocorrenciaRepository.AddAsync(ocorrencia);
            await _ocorrenciaRepository.SaveChangesAsync();

            // 5. Retorna DTO
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
