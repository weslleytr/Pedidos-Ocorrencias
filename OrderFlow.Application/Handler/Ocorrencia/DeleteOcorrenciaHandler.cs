using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Application.Core;
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

        public DeleteOcorrenciaHandler(
            IPedidoRepository pedidoRepository,
            IOcorrenciaRepository ocorrenciaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        public async Task<Result<DeleteOcorrenciaDto>> Handle(DeleteOcorrenciaDto dto)
        {
            try
            {
                // 1. Carrega o pedido com as ocorrências
                var pedido = await _pedidoRepository.GetPedidoByNumberAsync(dto.NumeroPedido);
                if (pedido == null)
                    return Result<DeleteOcorrenciaDto>.Failure(Error.NotFound("PedidoNaoEncontrado", "Pedido não encontrado."));

                // 2. Verifica se o pedido está finalizado
                if (pedido.IndEntregue)
                    return Result<DeleteOcorrenciaDto>.Failure(Error.Conflict("PedidoFinalizado", "Pedido já finalizado."));

                // 3. Localiza a ocorrência a ser excluída
                var ocorrencia = pedido.Ocorrencias.FirstOrDefault(o => o.IdOcorrencia == dto.IdOcorrencia);
                if (ocorrencia == null)
                    return Result<DeleteOcorrenciaDto>.Failure(Error.NotFound("OcorrenciaNaoEncontrada", "Ocorrência não encontrada."));

                // 4. Remove a ocorrência da lista do pedido
                pedido.Ocorrencias.Remove(ocorrencia);

                // 5. Atualiza o status do pedido, caso necessário
                pedido.IndEntregue = pedido.Ocorrencias.Any(o => o.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso);

                // 6. Remove a ocorrência do banco
                await _ocorrenciaRepository.RemoveAsync(ocorrencia);

                return Result<DeleteOcorrenciaDto>.Success(new DeleteOcorrenciaDto(
                        dto.NumeroPedido,
                        dto.IdOcorrencia
                ));
            }
            catch (InvalidOperationException ex)
            {
                // Captura erros de regra de negócio do domínio
                return Result<DeleteOcorrenciaDto>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                // Captura qualquer outro erro inesperado
                return Result<DeleteOcorrenciaDto>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }
    }
}
