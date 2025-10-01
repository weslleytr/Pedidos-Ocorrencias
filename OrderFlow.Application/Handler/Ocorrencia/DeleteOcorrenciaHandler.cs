using Microsoft.Extensions.Logging;
using OrderFlow.Application.Core;
using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Handler.Ocorrencia
{
    public class DeleteOcorrenciaHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;
        private readonly ILogger<DeleteOcorrenciaHandler> _logger;

        public DeleteOcorrenciaHandler(IPedidoRepository pedidoRepository,IOcorrenciaRepository ocorrenciaRepository, ILogger<DeleteOcorrenciaHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
            _logger = logger;
        }

        public async Task<Result<DeleteOcorrenciaDto>> Handle(DeleteOcorrenciaDto dto)
        {
            try
            {
                // --------------------------------
                // Valida o pedido existente
                // --------------------------------
                var pedido = await _pedidoRepository.GetPedidoByNumberAsync(dto.NumeroPedido);
                if (pedido == null)
                    return Result<DeleteOcorrenciaDto>.Failure(Error.NotFound("PedidoNaoEncontrado", "Pedido não encontrado."));

                // --------------------------------
                // Verifica se o pedido já foi finalizado
                // --------------------------------
                if (pedido.IndEntregue)
                    return Result<DeleteOcorrenciaDto>.Failure(Error.Conflict("PedidoFinalizado", "Pedido já finalizado."));

                // --------------------------------
                // Busca a ocorrência a ser removida
                // --------------------------------
                var ocorrencia = pedido.Ocorrencias.FirstOrDefault(o => o.IdOcorrencia == dto.IdOcorrencia);
                if (ocorrencia == null)
                    return Result<DeleteOcorrenciaDto>.Failure(Error.NotFound("OcorrenciaNaoEncontrada", "Ocorrência não encontrada."));

                // --------------------------------
                // Remove a ocorrência da lista do pedido
                // --------------------------------
                pedido.Ocorrencias.Remove(ocorrencia);

                // --------------------------------
                // Reaplica as regras de negócio ao pedido
                // --------------------------------
                pedido.IndEntregue = pedido.Ocorrencias.Any(o => o.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso);

                // --------------------------------
                // Remove a ocorrência do repositório
                // --------------------------------
                await _ocorrenciaRepository.RemoveAsync(ocorrencia);

                // --------------------------------
                // Retorna o DTO da ocorrência apagada
                // --------------------------------
                _logger.LogInformation("Ocorrência {IdOcorrencia} removida do pedido {NumeroPedido}", dto.IdOcorrencia, dto.NumeroPedido);
                return Result<DeleteOcorrenciaDto>.Success(new DeleteOcorrenciaDto(
                        dto.NumeroPedido,
                        dto.IdOcorrencia
                ));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Regra de negócio violada ao excluir ocorrência {IdOcorrencia} do pedido {NumeroPedido}", dto.IdOcorrencia, dto.NumeroPedido);
                return Result<DeleteOcorrenciaDto>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir ocorrência {IdOcorrencia} do pedido {NumeroPedido}", dto.IdOcorrencia, dto.NumeroPedido);
                return Result<DeleteOcorrenciaDto>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }
    }
}
