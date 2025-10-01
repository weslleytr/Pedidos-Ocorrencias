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
            // --------------------------------
            // Valida o tipo de ocorrência
            // --------------------------------
            if (!System.Enum.IsDefined(typeof(ETipoOcorrencia), novaOcorrencia.TipoOcorrencia))
                throw new InvalidOperationException("Tipo de ocorrência inválido.");

            // --------------------------------
            // Não permite adicionar ocorrências a um pedido já finalizado
            // --------------------------------
            if (IndEntregue)
                throw new InvalidOperationException("Não é possível adicionar ocorrências a um pedido já finalizado.");

            // --------------------------------
            // Busca a última ocorrência do mesmo tipo
            // --------------------------------
            var ultimaDoMesmoTipo = Ocorrencias
                .Where(o => o.TipoOcorrencia == novaOcorrencia.TipoOcorrencia)
                .OrderByDescending(o => o.HoraOcorrencia)
                .FirstOrDefault();

            // --------------------------------
            // Não permite adicionar ocorrências do mesmo tipo com intervalo menor que 10 minutos
            // --------------------------------
            if (ultimaDoMesmoTipo != null &&
                novaOcorrencia.HoraOcorrencia.Subtract(ultimaDoMesmoTipo.HoraOcorrencia).TotalMinutes < 10)
            {
                throw new InvalidOperationException("Ocorrências do mesmo tipo só podem ser cadastradas com intervalo mínimo de 10 minutos.");
            }

            // --------------------------------
            // Define se a ocorrência é finalizadora
            // --------------------------------
            if (Ocorrencias.Count == 1 || novaOcorrencia.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso)
            {
                novaOcorrencia.IndFinalizadora = true;
            }

            // --------------------------------
            // Atualiza o status do pedido se a ocorrência for EntregueComSucesso
            // --------------------------------
            if (novaOcorrencia.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso)
                IndEntregue = true;
            else if (Ocorrencias.Count >= 1)
                IndEntregue = false;

            // --------------------------------
            // Adiciona a nova ocorrência à lista
            // --------------------------------
            Ocorrencias.Add(novaOcorrencia);
        }


        public void ExcluirOcorrencia(int ocorrenciaId)
        {
            // --------------------------------
            // Não permite excluir ocorrências de um pedido já finalizado
            // --------------------------------
            if (IndEntregue)
                throw new InvalidOperationException("Não é possível excluir ocorrências de um pedido já finalizado.");

            // --------------------------------
            // Busca a ocorrência pelo ID
            // --------------------------------
            var ocorrencia = Ocorrencias.FirstOrDefault(o => o.IdOcorrencia == ocorrenciaId);

            // --------------------------------
            // Remove a ocorrência se encontrada
            // --------------------------------
            if (ocorrencia != null)
                Ocorrencias.Remove(ocorrencia);
        }
    }
}
