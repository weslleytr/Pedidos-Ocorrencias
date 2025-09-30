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
        Task<Pedido?> GetByIdAsync(int id);
        void AddAsync(Pedido pedido);
        Task SaveChangesAsync();
    }
}
