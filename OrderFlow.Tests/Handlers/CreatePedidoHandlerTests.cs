using Microsoft.Extensions.Logging;
using Moq;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Tests.Handlers
{
    public class CreatePedidoHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepoMock = new();
        private readonly Mock<ILogger<CreatePedidoHandler>> _loggerMock = new();
        private CreatePedidoHandler CreateHandler() => new CreatePedidoHandler(_pedidoRepoMock.Object, _loggerMock.Object);

        [Fact]
        public async Task Handle_PedidoExiste_DeveRetornarErro()
        {
            // ---------------------------------
            // Mock do repositório para indicar que o pedido já existe
            // ---------------------------------
            _pedidoRepoMock.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync(true);

            // ---------------------------------
            // Criação do handler e do DTO
            // ---------------------------------
            var handler = CreateHandler();
            var dto = new CreatePedidoDto(1);

            // ---------------------------------
            // Execução do handler
            // ---------------------------------
            var result = await handler.Handle(dto);

            // ---------------------------------
            // Verifica o resultado
            // ---------------------------------
            Assert.False(result.isSuccess);
            Assert.Equal("PedidoJaExiste", result.Error.Code);
            Assert.Equal("Número de Pedido já Existe.", result.Error.Message);
        }

        [Fact]
        public async Task Handle_PedidoNaoExiste_DeveCriarPedido()
        {
            // ---------------------------------
            // Mock do repositório para indicar que o pedido não existe
            // ---------------------------------
            _pedidoRepoMock.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync(false);
            _pedidoRepoMock.Setup(r => r.AddAsync(It.IsAny<Pedido>())).Returns(Task.CompletedTask);
            _pedidoRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // ---------------------------------
            // Criação do handler e do DTO
            // ---------------------------------
            var handler = CreateHandler();
            var dto = new CreatePedidoDto(1);

            // ---------------------------------
            // Execução do handler
            // ---------------------------------
            var result = await handler.Handle(dto);

            // ---------------------------------
            // Verifica o resultado
            // ---------------------------------
            Assert.Equal(1, result?.Value?.NumeroPedido);
        }
    }

}
