using OrderFlow.Application.Dtos;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Handler.Ocorrencia
{
    public class DeleteOcorrenciaHandler
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IOcorrenciaRepository _ocorrenciaRepository;

        public DeleteOcorrenciaHandler(
            IPedidoRepository pedidoRepository,
            IOcorrenciaRepository ocorrenciaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _ocorrenciaRepository = ocorrenciaRepository;
        }

        public async Task<bool> Handle(int numeroPedido, int idOcorrencia)
        {
            // 1. Carrega o pedido com as ocorrências
            var pedido = await _pedidoRepository.GetPedidoByNumberAsync(numeroPedido);
            if (pedido == null)
                throw new InvalidOperationException("Pedido não encontrado.");

            // 2. Verifica se o pedido está finalizado
            if (pedido.IndEntregue)
                throw new InvalidOperationException("Não é possível excluir ocorrências de um pedido finalizado.");

            // 3. Localiza a ocorrência a ser excluída
            var ocorrencia = pedido.Ocorrencias.FirstOrDefault(o => o.IdOcorrencia == idOcorrencia);
            if (ocorrencia == null)
                throw new InvalidOperationException("Ocorrência não encontrada.");

            // 4. Remove a ocorrência da lista do pedido
            pedido.Ocorrencias.Remove(ocorrencia);

            // 5. Atualiza o status do pedido, caso necessário
            pedido.IndEntregue = pedido.Ocorrencias.Any(o => o.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso);

            // 6. Remove a ocorrência do banco
            await _ocorrenciaRepository.RemoveAsync(ocorrencia);

            return true;
        }
    }
}
