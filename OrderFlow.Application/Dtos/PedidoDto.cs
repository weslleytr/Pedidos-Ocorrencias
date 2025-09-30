using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Dtos
{
    public record PedidoDto(
    int IdPedido,
    int NumeroPedido,
    List<OcorrenciaDto> Ocorrencias,
    bool IndEntregue,
    DateTime HoraPedido
);
}
