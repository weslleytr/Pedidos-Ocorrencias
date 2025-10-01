using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infra.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFlow.Infra.Repositories
{
    public class OcorrenciaRepository : IOcorrenciaRepository
    {
        private readonly AppDbContext _context;

        public OcorrenciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ocorrencia?> GetByIdAsync(int id)
        {
            return await _context.Ocorrencias
                                 .FirstOrDefaultAsync(o => o.IdOcorrencia == id);
        }

        public async Task<List<Ocorrencia>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.Ocorrencias
                                 .Where(o => o.PedidoId == pedidoId)
                                 .ToListAsync();
        }

        public async Task AddAsync(Ocorrencia ocorrencia)
        {
           await _context.Ocorrencias.AddAsync(ocorrencia);
        }

        public async Task RemoveAsync(Ocorrencia ocorrencia)
        {
            await _context.Ocorrencias.Where(x => x.IdOcorrencia == ocorrencia.IdOcorrencia).ExecuteDeleteAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
