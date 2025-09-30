using OrderFlow.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderFlow.Domain.Entities
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int NumeroPedido { get; set; }
        public DateTime HoraPedido { get; set; }
        public bool IndEntregue { get; set; }

        public List<Ocorrencia> Ocorrencias { get; set; } = new();

        public Pedido() { }

        public Pedido(int numeroPedido)
        {
            NumeroPedido = numeroPedido;
            HoraPedido = DateTime.UtcNow;
            IndEntregue = false;
        }

        public void AdicionarOcorrencia(Ocorrencia novaOcorrencia)
        {
            if (IndEntregue)
                throw new InvalidOperationException("Não é possível adicionar ocorrências a um pedido já finalizado.");

            // Valida intervalo de 10 minutos
            var ultimaDoMesmoTipo = Ocorrencias
                .Where(o => o.TipoOcorrencia == novaOcorrencia.TipoOcorrencia)
                .OrderByDescending(o => o.HoraOcorrencia)
                .FirstOrDefault();

            if (ultimaDoMesmoTipo != null &&
                novaOcorrencia.HoraOcorrencia.Subtract(ultimaDoMesmoTipo.HoraOcorrencia).TotalMinutes < 10)
            {
                throw new InvalidOperationException("Ocorrências do mesmo tipo só podem ser cadastradas com intervalo mínimo de 10 minutos.");
            }

            // Se for a segunda ocorrência → finalizadora
            if (Ocorrencias.Count == 1)
            {
                novaOcorrencia.IndFinalizadora = true;

                if (novaOcorrencia.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso)
                    IndEntregue = true;
                else
                    IndEntregue = false;
            }

            Ocorrencias.Add(novaOcorrencia);
        }

        public void ExcluirOcorrencia(int ocorrenciaId)
        {
            if (IndEntregue)
                throw new InvalidOperationException("Não é possível excluir ocorrências de um pedido já finalizado.");

            var ocorrencia = Ocorrencias.FirstOrDefault(o => o.IdOcorrencia == ocorrenciaId);

            if (ocorrencia != null)
                Ocorrencias.Remove(ocorrencia);
        }
    }
}
