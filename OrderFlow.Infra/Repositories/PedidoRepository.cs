using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Infra.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetPedidoByNumberAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Ocorrencias)
                .FirstOrDefaultAsync(p => p.NumeroPedido == id);
        }

        public async Task<bool> Exists(int numeroPedido)
        {
            return await _context.Pedidos.AnyAsync(p => p.NumeroPedido == numeroPedido);
        }

        public async Task<List<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Ocorrencias)
                .ToListAsync();
        }

        public async Task AddAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            _context.Pedidos.UpdateRange(pedido);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
