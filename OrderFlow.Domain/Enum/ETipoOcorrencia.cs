using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Domain.Enum
{
    public enum ETipoOcorrencia
    {
        EmRotaDeEntrega = 0,
        EntregueComSucesso = 1,
        ClienteAusente = 2,
        AvariaNoProduto = 3
    }
}
