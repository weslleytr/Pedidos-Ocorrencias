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

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.IdPedido);
                entity.ToTable("Pedidos");
            });

            modelBuilder.Entity<Ocorrencia>(entity =>
            {
                entity.HasKey(o => o.IdOcorrencia);
                entity.Property(o => o.TipoOcorrencia).HasConversion<int>();

                // FK para Pedido
                entity.HasOne(o => o.Pedido)
                      .WithMany(p => p.Ocorrencias)
                      .HasForeignKey(o => o.PedidoId)
                      .IsRequired();
            });
        }

    }
}
