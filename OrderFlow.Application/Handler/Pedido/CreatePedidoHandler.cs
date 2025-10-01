using Microsoft.Extensions.Logging;
using OrderFlow.Application.Core;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Handler.Pedido
{
    public class CreatePedidoHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<CreatePedidoHandler> _logger;

        public CreatePedidoHandler(IPedidoRepository pedidoRepository, ILogger<CreatePedidoHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }

        public async Task<Result<PedidoDto>> Handle(CreatePedidoDto dto)
        {
            try
            {
                // --------------------------------
                // Cria o pedido no domínio
                // --------------------------------
                var pedido = new OrderFlow.Domain.Entities.Pedido(dto.NumeroPedido);

                // --------------------------------
                // Verifica se o número do pedido já existe
                // --------------------------------
                if (await _pedidoRepository.Exists(pedido.NumeroPedido))
                    return Result<PedidoDto>.Failure(Error.NotFound("PedidoJaExiste", "Número de Pedido já Existe."));

                // --------------------------------
                // Adiciona o pedido ao repositório
                // --------------------------------
                await _pedidoRepository.AddAsync(pedido);

                // --------------------------------
                // Salva as alterações no banco
                // --------------------------------
                await _pedidoRepository.SaveChangesAsync();

                // --------------------------------
                // Retorna o pedido criado
                // --------------------------------
                _logger.LogInformation($"Pedido {pedido.NumeroPedido} criado com sucesso.");
                return Result<PedidoDto>.Success(new PedidoDto(
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
                ));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar pedido.");
                return Result<PedidoDto>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar pedido.");
                return Result<PedidoDto>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }
    }
}
