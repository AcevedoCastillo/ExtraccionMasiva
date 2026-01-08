using CargaMasivaDato.Data.Context;
using CargaMasivaDatos.Core.Interfaces;
using CargaMasivaDatos.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDato.Data.Repositories
{
    public class RepositorioDestino : IRepositorioDestino
    {
        private readonly DestinoDbContext _context;

        public RepositorioDestino(DestinoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> GuardarJSONAsync(int pedidoId, string jsonData, int loteId)
        {
            try
            {
                var pedidoJson = new PedidoJSON
                {
                    PedidoID = pedidoId,
                    JSONData = jsonData,
                    FechaProcesamiento = DateTime.Now,
                    LoteID = loteId,
                    Estado = "Procesado"
                };

                _context.PedidosJSON.Add(pedidoJson);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> RegistrarLogProcesamientoAsync(LogProcesamiento log)
        {
            _context.LogProcesamiento.Add(log);
            await _context.SaveChangesAsync();
            return log.LogID;
        }

        public async Task<bool> ActualizarLogProcesamientoAsync(LogProcesamiento log)
        {
            try
            {
                _context.Entry(log).State = System.Data.Entity.EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}