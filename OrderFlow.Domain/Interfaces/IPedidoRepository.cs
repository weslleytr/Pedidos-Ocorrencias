using OrderFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetPedidoByNumberAsync(int id);
        Task<bool> Exists(int id);
        Task AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task<List<Pedido>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
