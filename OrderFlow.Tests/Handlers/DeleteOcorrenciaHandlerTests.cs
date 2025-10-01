using Moq;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Tests.Handlers
{
    public class DeleteOcorrenciaHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepoMock = new();
        private readonly Mock<IOcorrenciaRepository> _ocorrenciaRepoMock = new();

        private DeleteOcorrenciaHandler CreateHandler() =>
            new DeleteOcorrenciaHandler(_pedidoRepoMock.Object, _ocorrenciaRepoMock.Object);

        [Fact]
        public async Task Handle_PedidoNaoExiste_DeveLancarExcecao()
        {
            _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(It.IsAny<int>()))
                           .ReturnsAsync((Pedido?)null);

            var handler = CreateHandler();
            var dto = new DeleteOcorrenciaDto(1, 1 );

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(dto));
        }

        [Fact]
        public async Task Handle_OcorrenciaExiste_DeveRemover()
        {
            var pedido = new Pedido(123);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega) { IdOcorrencia = 1 };
            pedido.AdicionarOcorrencia(ocorrencia);

            _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(123))
                           .ReturnsAsync(pedido);

            var handler = CreateHandler();
            var dto = new DeleteOcorrenciaDto(123, 1 );

            var result = await handler.Handle(dto);

            Assert.Empty(pedido.Ocorrencias);
            Assert.True(result);
        }
    }

}
