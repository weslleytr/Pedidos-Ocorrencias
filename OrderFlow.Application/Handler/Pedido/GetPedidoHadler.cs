using Microsoft.Extensions.Logging;
using OrderFlow.Application.Core;
using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Handler.Pedido
{
    public class GetPedidoHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<GetPedidoHandler> _logger;
        public GetPedidoHandler(IPedidoRepository pedidoRepository, ILogger<GetPedidoHandler> logger)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }

        public async Task<Result<PedidoDto>> HandleByNumber(int numeroPedido)
        {
            try
            {
                // --------------------------------
                // Busca o pedido pelo número
                // --------------------------------
                var pedido = await _pedidoRepository.GetPedidoByNumberAsync(numeroPedido);

                // --------------------------------
                // Verifica se o pedido foi encontrado
                // --------------------------------
                if (pedido == null)
                    return Result<PedidoDto>.Failure(Error.NotFound("PedidoNaoEncontrado", "Pedido não encontrado."));

                // --------------------------------
                // Retorna o pedido encontrado
                // --------------------------------
                _logger.LogInformation($"Pedido {pedido.NumeroPedido} encontrado com sucesso.");
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
                _logger.LogError(ex, "Erro ao buscar pedido por número.");
                return Result<PedidoDto>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao buscar pedido por número.");
                return Result<PedidoDto>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }

        public async Task<Result<List<PedidoDto>>> HandleAll()
        {
            try
            {
                // --------------------------------
                // Busca todos os pedidos
                // --------------------------------
                var pedidos = await _pedidoRepository.GetAllAsync();

                // --------------------------------
                // Verifica se algum pedido foi encontrado
                // --------------------------------
                if (pedidos == null || !pedidos.Any())
                    return Result<List<PedidoDto>>.Failure(Error.NotFound("NenhumPedidoEncontrado", "Nenhum Pedido encontrado."));

                // --------------------------------
                // Retorna os pedidos encontrados
                // --------------------------------
                _logger.LogInformation($"Total de {pedidos.Count} pedidos encontrados com sucesso.");
                var pedidosDto = pedidos.Select(p => new PedidoDto(
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

                return Result<List<PedidoDto>>.Success(pedidosDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os pedidos.");
                return Result<List<PedidoDto>>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao buscar todos os pedidos.");
                return Result<List<PedidoDto>>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }
    }
}
