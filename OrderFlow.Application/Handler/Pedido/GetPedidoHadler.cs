using OrderFlow.Application.Core;
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

        public async Task<Result<PedidoDto>> HandleByNumber(int numeroPedido)
        {
            try
            {
                // Buscar o pedido no repositório
                var pedido = await _pedidoRepository.GetPedidoByNumberAsync(numeroPedido);
                if (pedido == null)
                    return Result<PedidoDto>.Failure(Error.NotFound("PedidoNaoEncontrado", "Pedido não encontrado."));

                // Mapear entidade para DTO
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
                return Result<PedidoDto>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                return Result<PedidoDto>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }

        public async Task<Result<List<PedidoDto>>> HandleAll()
        {
            try
            {
                var pedidos = await _pedidoRepository.GetAllAsync();
                if (pedidos == null || !pedidos.Any())
                    return Result<List<PedidoDto>>.Failure(Error.NotFound("NenhumPedidoEncontrado", "Nenhum Pedido encontrado."));

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
                return Result<List<PedidoDto>>.Failure(Error.Validation("RegraNegocio", ex.Message));
            }
            catch (Exception ex)
            {
                return Result<List<PedidoDto>>.Failure(Error.Failure("ErroInterno", ex.Message));
            }
        }
    }
}
