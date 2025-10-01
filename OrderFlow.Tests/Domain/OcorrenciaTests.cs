using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enum;

namespace OrderFlow.Domain.Tests
{
    public class OcorrenciaTests
    {
        [Fact]
        public void CriarOcorrencia_ComTipo_DeveDefinirHoraEFinalizadoraFalse()
        {
            // --------------------------------
            // Arrange & Act
            // --------------------------------
            var ocorrencia = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega);

            // --------------------------------
            // Verifica se as propriedades foram inicializadas corretamente
            // --------------------------------
            Assert.Equal(ETipoOcorrencia.EmRotaDeEntrega, ocorrencia.TipoOcorrencia);
            Assert.False(ocorrencia.IndFinalizadora);
            Assert.True((DateTime.UtcNow - ocorrencia.HoraOcorrencia).TotalSeconds < 2);
        }

        [Fact]
        public void CriarOcorrencia_ComHoraNoFuturo_DeveLancarExcecao()
        {
            // --------------------------------
            // Arrange
            // --------------------------------
            var futureDate = DateTime.Now.AddMinutes(10);

            // --------------------------------
            // Act & Assert
            // --------------------------------
            Assert.Throws<ArgumentException>(() =>
                new Ocorrencia(1, ETipoOcorrencia.EmRotaDeEntrega, futureDate)
            );
        }

        [Fact]
        public void CriarOcorrencia_Valida_DeveDefinirPropriedadesCorretamente()
        {
            // --------------------------------
            // Arrange
            // --------------------------------
            var data = DateTime.Now.AddMinutes(-5);

            // --------------------------------
            // Act
            // --------------------------------
            var ocorrencia = new Ocorrencia(1, ETipoOcorrencia.EmRotaDeEntrega, data);

            // --------------------------------
            // Verifica se as propriedades foram definidas corretamente
            // --------------------------------
            Assert.Equal(1, ocorrencia.IdOcorrencia);
            Assert.Equal(ETipoOcorrencia.EmRotaDeEntrega, ocorrencia.TipoOcorrencia);
            Assert.Equal(data, ocorrencia.HoraOcorrencia);
            Assert.False(ocorrencia.IndFinalizadora);
        }
    }
}
