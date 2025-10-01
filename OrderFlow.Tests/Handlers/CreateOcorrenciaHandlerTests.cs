using Microsoft.Extensions.Logging;
using Moq;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;
using OrderFlow.Application.Handler.Pedido;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;

public class CreateOcorrenciaHandlerTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepoMock = new();
    private readonly Mock<IOcorrenciaRepository> _ocorrenciaRepoMock = new();
    private readonly Mock<ILogger<CreateOcorrenciaHandler>> _loggerMock = new();

    private CreateOcorrenciaHandler CreateHandler() =>
        new CreateOcorrenciaHandler(_pedidoRepoMock.Object, _ocorrenciaRepoMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Handle_PedidoNaoExiste_DeveRetornarNotFound()
    {
        // --------------------------------
        // Mock do repositório para retornar null
        // --------------------------------
        _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(It.IsAny<int>()))
                       .ReturnsAsync((Pedido?)null);

        // ---------------------------------
        // Cria o handler e o DTO de entrada
        // ---------------------------------
        var handler = CreateHandler();
        var dto = new CreateOcorrenciaDto(1, ETipoOcorrencia.EmRotaDeEntrega);

        // ---------------------------------
        // Executa o método Handle
        // ---------------------------------
        var result = await handler.Handle(dto);

        // ---------------------------------
        // Verifica o resultado
        // ---------------------------------
        Assert.False(result.isSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("PedidoNaoEncontrado", result.Error.Code);
        Assert.Equal("Pedido não encontrado.", result.Error.Message);
    }

    [Fact]
    public async Task Handle_PedidoExiste_DeveAdicionarOcorrencia()
    {
        // --------------------------------
        // Mock do repositório para retornar um pedido existente
        // --------------------------------
        var pedido = new Pedido(123);
        _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(123))
                       .ReturnsAsync(pedido);

        // ---------------------------------
        // Cria o handler e o DTO de entrada
        // ---------------------------------
        var handler = CreateHandler();
        var dto = new CreateOcorrenciaDto(123, ETipoOcorrencia.EmRotaDeEntrega);

        // ---------------------------------
        // Executa o método Handle
        // ---------------------------------
        var result = await handler.Handle(dto);

        // ---------------------------------
        // Verifica o resultado
        // ---------------------------------
        Assert.Single(pedido.Ocorrencias);
        Assert.Equal(ETipoOcorrencia.EmRotaDeEntrega, result?.Value?.TipoOcorrencia);
    }
}
