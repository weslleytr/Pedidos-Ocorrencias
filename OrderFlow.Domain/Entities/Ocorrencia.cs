using OrderFlow.Domain.Enum;
using System;

namespace OrderFlow.Domain.Entities
{
    public class Ocorrencia
    {
        public int IdOcorrencia { get; set; }
        public ETipoOcorrencia TipoOcorrencia { get; set; }
        public DateTime HoraOcorrencia { get; set; }
        public bool IndFinalizadora { get; set; }

        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        public Ocorrencia() { }

        public Ocorrencia(ETipoOcorrencia tipoOcorrencia)
        {
            TipoOcorrencia = tipoOcorrencia;
            HoraOcorrencia = DateTime.UtcNow;
            IndFinalizadora = false;
        }

        public Ocorrencia(int idOcorrencia, ETipoOcorrencia tipoOcorrencia, DateTime horaOcorrencia)
        {
            // --------------------------------
            // Não permite registrar ocorrências com hora inválida
            // --------------------------------
            if (horaOcorrencia > DateTime.Now)
                throw new ArgumentException("A hora da ocorrência não pode ser no futuro.");

            IdOcorrencia = idOcorrencia;
            TipoOcorrencia = tipoOcorrencia;
            HoraOcorrencia = horaOcorrencia;
            IndFinalizadora = false;
        }
    }
}
