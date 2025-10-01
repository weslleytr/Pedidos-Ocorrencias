using OrderFlow.Application.Core;
using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Entities;
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

        public async Task<Result<PedidoDto>> Handle(CreatePedidoDto dto)
        {
            try
            {
                // 1. Cria pedido no domínio
                var pedido = new OrderFlow.Domain.Entities.Pedido(dto.NumeroPedido);

                if (await _pedidoRepository.Exists(pedido.NumeroPedido))
                    return Result<PedidoDto>.Failure(Error.NotFound("PedidoJaExiste", "Número de Pedido já Existe."));
                // 3. Persiste pedido via repositório
                await _pedidoRepository.AddAsync(pedido);
                await _pedidoRepository.SaveChangesAsync();

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
    }
}
