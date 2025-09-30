using OrderFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Domain.Interfaces
{
    public interface IOcorrenciaRepository
    {
        Task<Ocorrencia> GetByIdAsync(int id);
        void AddAsync(Ocorrencia ocorrencia);
        void RemoveAsync(Ocorrencia ocorrencia);
        Task SaveChangesAsync();
        Task<List<Ocorrencia>> GetByPedidoIdAsync(int pedidoId);
    }
}
