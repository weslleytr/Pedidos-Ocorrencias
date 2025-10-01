using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infra.Data
{
    public class AppDbContext : DbContext
    {
        // --------------------------------
        // Construtor que recebe opções do DbContext
        // --------------------------------
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // --------------------------------
        // DbSet para acessar a tabela de Pedidos
        // --------------------------------
        public DbSet<Pedido> Pedidos => Set<Pedido>();

        // --------------------------------
        // DbSet para acessar a tabela de Ocorrencias
        // --------------------------------
        public DbSet<Ocorrencia> Ocorrencias => Set<Ocorrencia>();

        // --------------------------------
        // Configurações adicionais do modelo
        // --------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------------------------------
            // Configuração da entidade Pedido
            // --------------------------------
            modelBuilder.Entity<Pedido>(entity =>
            {
                // --------------------------------
                // Define a chave primária
                // --------------------------------
                entity.HasKey(p => p.IdPedido);

                // --------------------------------
                // Define o nome da tabela no banco
                // --------------------------------
                entity.ToTable("Pedidos");
            });

            // --------------------------------
            // Configuração da entidade Ocorrencia
            // --------------------------------
            modelBuilder.Entity<Ocorrencia>(entity =>
            {
                // --------------------------------
                // Define a chave primária
                // --------------------------------
                entity.HasKey(o => o.IdOcorrencia);

                // --------------------------------
                // Converte o enum TipoOcorrencia para int no banco
                // --------------------------------
                entity.Property(o => o.TipoOcorrencia).HasConversion<int>();

                // --------------------------------
                // Configuração da relação com Pedido (FK)
                // --------------------------------
                entity.HasOne(o => o.Pedido)
                      .WithMany(p => p.Ocorrencias)
                      .HasForeignKey(o => o.PedidoId)
                      .IsRequired();
            });
        }

    }
}
