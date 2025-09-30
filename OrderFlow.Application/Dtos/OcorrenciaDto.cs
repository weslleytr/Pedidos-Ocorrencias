using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFlow.Domain.Enum;

namespace OrderFlow.Application.Dtos
{
    public record OcorrenciaDto(
    int IdOcorrencia,
    ETipoOcorrencia TipoOcorrencia,
    bool IndFinalizadora,
    DateTime HoraOcorrencia
);
}
