using Moq;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Handler.Ocorrencia;

public class CreateOcorrenciaHandlerTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepoMock = new();
    private readonly Mock<IOcorrenciaRepository> _ocorrenciaRepoMock = new();

    private CreateOcorrenciaHandler CreateHandler() =>
        new CreateOcorrenciaHandler(_pedidoRepoMock.Object, _ocorrenciaRepoMock.Object);

    [Fact]
    public async Task Handle_PedidoNaoExiste_DeveLancarExcecao()
    {
        _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(It.IsAny<int>()))
                       .ReturnsAsync((Pedido?)null);

        var handler = CreateHandler();
        var dto = new CreateOcorrenciaDto(1, ETipoOcorrencia.EmRotaDeEntrega);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(dto));
    }

    [Fact]
    public async Task Handle_PedidoExiste_DeveAdicionarOcorrencia()
    {
        var pedido = new Pedido(123);
        _pedidoRepoMock.Setup(r => r.GetPedidoByNumberAsync(123))
                       .ReturnsAsync(pedido);

        var handler = CreateHandler();
        var dto = new CreateOcorrenciaDto(123, ETipoOcorrencia.EmRotaDeEntrega);

        var result = await handler.Handle(dto);

        Assert.Single(pedido.Ocorrencias);
        Assert.Equal(ETipoOcorrencia.EmRotaDeEntrega, result.TipoOcorrencia);
    }
}
