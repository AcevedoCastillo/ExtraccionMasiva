using CargaMasivaDato.Data.Context;
using CargaMasivaDatos.Core.Interfaces;
using CargaMasivaDatos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDato.Data.Repositories
{
    public class RepositorioOrigen : IRepositorioOrigen
    {
        private readonly OrigenDbContext _context;

        public RepositorioOrigen(OrigenDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> ObtenerPedidosPendientesAsync(int cantidad)
        {
            return await _context.Pedidos
                .Where(p => !p.Procesado)
                .OrderBy(p => p.FechaPedido)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task<bool> MarcarPedidoComoProcesadoAsync(int pedidoId)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(pedidoId);
                if (pedido == null)
                    return false;

                pedido.Procesado = true;
                pedido.Estado = "Procesado";

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Pedido> ObtenerPedidoCompletoAsync(int pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles.Select(d => d.Producto))
                .FirstOrDefaultAsync(p => p.PedidoID == pedidoId);
        }
    }
}
