using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Dtos
{
    public record DeleteOcorrenciaDto(
        int NumeroPedido, 
        int IdOcorrencia
    );
}
