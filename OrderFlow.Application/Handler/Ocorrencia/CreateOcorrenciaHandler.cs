using Microsoft.Extensions.Logging;
using OrderFlow.Application.Core;
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
        private readonly ILogger<CreateOcorrenciaHandler> _logger;
        public CreateOcorrenciaHandler(IPedidoRepository pedidoRepository,IOcorrenciaRepository ocorrenciaRepository, ILogger<CreateOcorrenciaHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
            _logger = logger;
        }

        public async Task<Result<OcorrenciaDto>> Handle(CreateOcorrenciaDto dto)
        {
            try
            {
                // --------------------------------
                // Valida o pedido existente
                // --------------------------------
                var pedido = await _pedidoRepository.GetPedidoByNumberAsync(dto.NumeroPedido);
                if (pedido == null)
                    return Result<OcorrenciaDto>.Failure(Error.NotFound("PedidoNaoEncontrado", "Pedido não encontrado."));

                // --------------------------------
                // Cria a nova ocorrência
                // --------------------------------
                var ocorrencia = new OrderFlow.Domain.Entities.Ocorrencia(dto.TipoOcorrencia)
                {
                    PedidoId = pedido.IdPedido
                };

                // --------------------------------
                // Aplica as regras de negócio ao pedido
                // --------------------------------
                pedido.AdicionarOcorrencia(ocorrencia);

                // --------------------------------
                // Adiciona a ocorrência ao repositório
                // --------------------------------
                await _ocorrenciaRepository.AddAsync(ocorrencia);

                // --------------------------------
                // Atualiza o pedido (status e lista de ocorrências)
                // --------------------------------
                await _pedidoRepository.UpdateAsync(pedido);

                // --------------------------------
                // Salva as alterações no banco
                // --------------------------------
                await _ocorrenciaRepository.SaveChangesAsync();
                await _pedidoRepository.SaveChangesAsync();

                // --------------------------------
                // Retorna o DTO da ocorrência criada
                // --------------------------------
                _logger.LogInformation("Ocorrência do tipo {TipoOcorrencia} criada para o pedido {NumeroPedido}", dto.TipoOcorrencia, dto.NumeroPedido);
                return Result<OcorrenciaDto>.Success(new OcorrenciaDto(
                    ocorrencia.IdOcorrencia,
                    ocorrencia.TipoOcorrencia,
                    ocorrencia.IndFinalizadora,
                    ocorrencia.HoraOcorrencia,
                    ocorrencia.PedidoId
                ));

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Regra de negócio violada ao criar ocorrência para o pedido {NumeroPedido}", dto.NumeroPedido);
                return Result<OcorrenciaDto>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar ocorrência para o pedido {NumeroPedido}", dto.NumeroPedido);
                return Result<OcorrenciaDto>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }
    }
}
