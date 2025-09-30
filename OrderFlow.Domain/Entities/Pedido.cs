using OrderFlow.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Domain.Entities
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int NumeroPedido { get; set; }
        public DateTime HoraPedido { get; set; }
        public bool IndEntregue { get; set; }

        private readonly List<Ocorrencia> _ocorrencias = new List<Ocorrencia>();
        public IReadOnlyList<Ocorrencia> Ocorrencias => _ocorrencias;
        public Pedido() { }

        public Pedido(int idPedido, int numeroPedido, DateTime horaPedido)
        {
            if (horaPedido > DateTime.Now)
                throw new ArgumentException("A hora do pedido não pode ser no futuro.");
            IdPedido = idPedido;
            NumeroPedido = numeroPedido;
            HoraPedido = horaPedido;
            IndEntregue = false;
        }

        public Pedido(int numeroPedido)
        {
            NumeroPedido = numeroPedido;
            HoraPedido = DateTime.UtcNow;
            IndEntregue = false;
        }

        public void AdicionarOcorrencia(Ocorrencia novaOcorrencia)
        {
            if (IndEntregue)
            {
                throw new Exception("Não é possível adicionar ocorrências a um pedido que já foi entregue ou finalizado.");
            }

            var ultimaOcorrenciaDoTipo = _ocorrencias
                .Where(o => o.TipoOcorrencia == novaOcorrencia.TipoOcorrencia)
                .OrderByDescending(o => o.HoraOcorrencia)
                .FirstOrDefault();

            if (ultimaOcorrenciaDoTipo != null &&
                novaOcorrencia.HoraOcorrencia.Subtract(ultimaOcorrenciaDoTipo.HoraOcorrencia).TotalMinutes < 10)
            {
                throw new Exception("Ocorrências do mesmo tipo só podem ser cadastradas com intervalo mínimo de 10 minutos.");
            }

            _ocorrencias.Add(novaOcorrencia);

            AnalisarStatusDoPedido();
        }

        public void ExcluirOcorrencia(int ocorrenciaId)
        {
            if (IndEntregue)
            {
                throw new Exception("Não é possível excluir ocorrências de um pedido que já foi entregue ou finalizado.");
            }

            var ocorrenciaParaRemover = _ocorrencias.FirstOrDefault(o => o.IdOcorrencia == ocorrenciaId);

            if (ocorrenciaParaRemover != null)
            {
                _ocorrencias.Remove(ocorrenciaParaRemover);
                AnalisarStatusDoPedido();
            }
        }

        private void AnalisarStatusDoPedido()
        {
            if (_ocorrencias.Count == 2)
            {
                var ocorrenciaFinalizadora = _ocorrencias.Last();
                if (ocorrenciaFinalizadora.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso)
                {
                    IndEntregue = true;
                }
                else
                {
                    IndEntregue = false;
                }
            }
        }
    }
}
