using OrderFlow.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Domain.Entities
{
    public class Ocorrencia
    {
        public int IdOcorrencia { get; set; }
        public ETipoOcorrencia TipoOcorrencia { get; set; }
        public DateTime HoraOcorrencia { get; set; }
        public bool IndFinalizadora { get; set; }

        private Ocorrencia() { }

        public Ocorrencia(ETipoOcorrencia tipoOcorrencia)
        {
            TipoOcorrencia = tipoOcorrencia;
            HoraOcorrencia = DateTime.UtcNow;
            IndFinalizadora = false; // será definido pelo método de domínio
        }

        public Ocorrencia(int idOcorrencia, ETipoOcorrencia tipoOcorrencia, DateTime horaOcorrencia, bool indFinalizadora)
        {
            if(horaOcorrencia > DateTime.Now)
                throw new ArgumentException("A hora da ocorrência não pode ser no futuro.");

            IdOcorrencia = idOcorrencia;
            TipoOcorrencia = tipoOcorrencia;
            HoraOcorrencia = horaOcorrencia;
            IndFinalizadora = indFinalizadora;
        }
    }
}
