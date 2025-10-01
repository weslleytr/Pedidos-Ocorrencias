using Microsoft.Extensions.Logging;
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
        private readonly Mock<ILogger<DeleteOcorrenciaHandler>> _loggerMock = new();

        private DeleteOcorrenciaHandler CreateHandler() =>
            new DeleteOcorrenciaHandler(_pedidoRepoMock.Object, _ocorrenciaRepoMock.Object, _loggerMock.Object);

        [Fact]
        public async Task Handle_PedidoNaoExiste_DeveRetornarNotFound()
        {
            // ---------------------------------
            // Configura o mock para retornar null, simulando que o pedido não existe
            // ---------------------------------
            _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(It.IsAny<int>()))
                           .ReturnsAsync((Pedido?)null);

            // ---------------------------------
            // Cria o handler e o DTO de entrada
            // ---------------------------------
            var handler = CreateHandler();
            var dto = new DeleteOcorrenciaDto(1, 1);

            // ---------------------------------
            // Executa o método Handle
            // ---------------------------------
            var result = await handler.Handle(dto);

            // ---------------------------------
            // Verifica o resultado
            // ---------------------------------
            Assert.False(result.isSuccess);
            Assert.Equal("PedidoNaoEncontrado", result.Error.Code);
            Assert.Equal("Pedido não encontrado.", result.Error.Message);
        }

        [Fact]
        public async Task Handle_OcorrenciaExiste_DeveRemover()
        {
            // ---------------------------------
            // Configura o mock para retornar um pedido com uma ocorrência
            // ---------------------------------
            var pedido = new Pedido(123);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega) { IdOcorrencia = 1 };

            // ---------------------------------
            // Adiciona a ocorrência ao pedido
            // ---------------------------------
            pedido.AdicionarOcorrencia(ocorrencia);

            // ---------------------------------
            // Configura o mock para retornar o pedido com a ocorrência
            // ---------------------------------
            _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(123))
                           .ReturnsAsync(pedido);

            // ---------------------------------
            // Configura o mock para simular a remoção da ocorrência
            // ---------------------------------
            var handler = CreateHandler();
            var dto = new DeleteOcorrenciaDto(123, 1 );

            // ---------------------------------
            // Executa o método Handle
            // ---------------------------------
            var result = await handler.Handle(dto);

            // ---------------------------------
            // Verifica o resultado
            // ---------------------------------
            Assert.Empty(pedido.Ocorrencias);
            Assert.True(result.isSuccess);
        }
    }

}
