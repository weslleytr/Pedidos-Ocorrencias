using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;

namespace OrderFlow.Domain.Tests
{
    public class PedidoTests
    {
        [Fact]
        public void CriarPedido_DeveInicializarCorretamente()
        {
            // --------------------------------
            // Arrange & Act
            // --------------------------------
            var pedido = new Pedido(123);

            // --------------------------------
            // Verifica o resultado
            // --------------------------------
            Assert.Equal(123, pedido.NumeroPedido);
            Assert.False(pedido.IndEntregue);
            Assert.Empty(pedido.Ocorrencias);
        }

        [Fact]
        public void AdicionarOcorrencia_DeveAdicionarComSucesso()
        {
            // --------------------------------
            // Arrange & Act
            // --------------------------------
            var pedido = new Pedido(1);

            // --------------------------------
            // Adiciona uma ocorrência válida
            // --------------------------------
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);

            // --------------------------------
            // Act
            // --------------------------------
            pedido.AdicionarOcorrencia(ocorrencia);

            // --------------------------------
            // Verifica o resultado
            // --------------------------------
            Assert.Single(pedido.Ocorrencias);
            Assert.Contains(ocorrencia, pedido.Ocorrencias);
        }

        [Fact]
        public void AdicionarOcorrencia_SegundoMesmoTipoMenosDe10Min_DeveLancarExcecao()
        {
            // --------------------------------
            // Arrange
            // --------------------------------
            var pedido = new Pedido(1);
            var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);
            pedido.AdicionarOcorrencia(ocorrencia1);

            var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega)
            {
                HoraOcorrencia = ocorrencia1.HoraOcorrencia.AddMinutes(5)
            };

            // --------------------------------
            // Act & Assert
            // --------------------------------
            Assert.Throws<InvalidOperationException>(() => pedido.AdicionarOcorrencia(ocorrencia2));
        }

        [Fact]
        public void AdicionarOcorrencia_EntregueComSucesso_DeveFinalizarPedido()
        {
            // --------------------------------
            // Arrange
            // --------------------------------
            var pedido = new Pedido(1);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso);

            // --------------------------------
            // Act
            // --------------------------------
            pedido.AdicionarOcorrencia(ocorrencia);

            // --------------------------------
            // Verifica se o pedido foi finalizado
            // --------------------------------
            Assert.True(pedido.IndEntregue);
            Assert.True(ocorrencia.IndFinalizadora);
        }

        [Fact]
        public void ExcluirOcorrencia_DeveRemoverCorretamente()
        {
            // --------------------------------
            // Arrange
            // --------------------------------
            var pedido = new Pedido(1);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega) { IdOcorrencia = 99 };
            pedido.AdicionarOcorrencia(ocorrencia);

            // --------------------------------
            // Act
            // --------------------------------
            pedido.ExcluirOcorrencia(99);

            // --------------------------------
            // Verifica se a ocorrência foi removida
            // --------------------------------
            Assert.Empty(pedido.Ocorrencias);
        }

        [Fact]
        public void AdicionarOcorrencia_PedidoJaEntregue_DeveLancarExcecao()
        {
            // --------------------------------
            // Arrange
            // --------------------------------
            var pedido = new Pedido(1);
            var entrega = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso);
            pedido.AdicionarOcorrencia(entrega);

            var novaOcorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);

            // --------------------------------
            // Act & Assert
            // --------------------------------
            Assert.Throws<InvalidOperationException>(() => pedido.AdicionarOcorrencia(novaOcorrencia));
        }
    }
}
