using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;

namespace OrderFlow.Domain.Tests
{
    public class PedidoTests
    {
        [Fact]
        public void CriarPedido_DeveInicializarCorretamente()
        {
            var pedido = new Pedido(123);

            Assert.Equal(123, pedido.NumeroPedido);
            Assert.False(pedido.IndEntregue);
            Assert.Empty(pedido.Ocorrencias);
        }

        [Fact]
        public void AdicionarOcorrencia_DeveAdicionarComSucesso()
        {
            var pedido = new Pedido(1);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);

            pedido.AdicionarOcorrencia(ocorrencia);

            Assert.Single(pedido.Ocorrencias);
            Assert.Contains(ocorrencia, pedido.Ocorrencias);
        }

        [Fact]
        public void AdicionarOcorrencia_SegundoMesmoTipoMenosDe10Min_DeveLancarExcecao()
        {
            var pedido = new Pedido(1);
            var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);
            pedido.AdicionarOcorrencia(ocorrencia1);

            var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega)
            {
                HoraOcorrencia = ocorrencia1.HoraOcorrencia.AddMinutes(5)
            };

            Assert.Throws<InvalidOperationException>(() => pedido.AdicionarOcorrencia(ocorrencia2));
        }

        [Fact]
        public void AdicionarOcorrencia_EntregueComSucesso_DeveFinalizarPedido()
        {
            var pedido = new Pedido(1);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso);

            pedido.AdicionarOcorrencia(ocorrencia);

            Assert.True(pedido.IndEntregue);
            Assert.True(ocorrencia.IndFinalizadora);
        }

        [Fact]
        public void ExcluirOcorrencia_DeveRemoverCorretamente()
        {
            var pedido = new Pedido(1);
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega) { IdOcorrencia = 99 };

            pedido.AdicionarOcorrencia(ocorrencia);
            pedido.ExcluirOcorrencia(99);

            Assert.Empty(pedido.Ocorrencias);
        }

        [Fact]
        public void AdicionarOcorrencia_PedidoJaEntregue_DeveLancarExcecao()
        {
            var pedido = new Pedido(1);
            var entrega = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso);
            pedido.AdicionarOcorrencia(entrega);

            var novaOcorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);

            Assert.Throws<InvalidOperationException>(() => pedido.AdicionarOcorrencia(novaOcorrencia));
        }
    }
}
