using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<Ocorrencia> Ocorrencias => Set<Ocorrencia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração Ocorrencia
            modelBuilder.Entity<Ocorrencia>(entity =>
            {
                entity.HasKey(o => o.IdOcorrencia);
                entity.Property(o => o.TipoOcorrencia).HasConversion<int>();

                // Define FK para Pedido
                entity.HasOne<Pedido>()
                      .WithMany(p => p.Ocorrencias)
                      .HasForeignKey("PedidoId")
                      .IsRequired();
            });

            // Configuração Pedido
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.IdPedido);
                entity.ToTable("Pedidos");
            });
        }
    }
}
