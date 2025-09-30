using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<Ocorrencia> Ocorrencias => Set<Ocorrencia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ocorrencia>()
                .Property(o => o.TipoOcorrencia)
                .HasConversion<int>();

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Ocorrencias)
                .WithOne()
                .HasForeignKey("PedidoId")
                .IsRequired();


            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Ocorrencias)
                .WithOne()
                .HasForeignKey("PedidoId")
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
