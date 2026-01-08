using CargaMasivaDatos.Core.Interfaces;
using CargaMasivaDatos.Core.Models;
using CargaMasivaDatos.Core.Models.DTOs;
using CargaMasivaDatos.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Data.Repositories
{
    public class RepositorioConfiguracion : IRepositorioConfiguracion
    {
        private readonly DestinoDbContext _context;

        public RepositorioConfiguracion(DestinoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConfiguracionServicio>> ObtenerConfiguracionesAsync()
        {
            return await _context.ConfiguracionServicio.ToListAsync();
        }

        public async Task<ConfiguracionServicio> ObtenerConfiguracionPorNombreAsync(string nombreConfig)
        {
            return await _context.ConfiguracionServicio
                .FirstOrDefaultAsync(c => c.NombreConfig == nombreConfig);
        }

        public async Task<ConfiguracionDTO> ObtenerConfiguracionServicioAsync()
        {
            var configs = await ObtenerConfiguracionesAsync();

            var dto = new ConfiguracionDTO
            {
                HoraInicio = TimeSpan.Parse(ObtenerValor(configs, "HoraInicio", "08:00")),
                HoraFin = TimeSpan.Parse(ObtenerValor(configs, "HoraFin", "18:00")),
                FrecuenciaMinutos = int.Parse(ObtenerValor(configs, "FrecuenciaMinutos", "30")),
                TamañoLote = int.Parse(ObtenerValor(configs, "TamañoLote", "100")),
                MaxReintentos = int.Parse(ObtenerValor(configs, "MaxReintentos", "3")),
                TiempoEsperaReintento = int.Parse(ObtenerValor(configs, "TiempoEsperaReintento", "5")),
                ServicioActivo = bool.Parse(ObtenerValor(configs, "ServicioActivo", "true"))
            };

            return dto;
        }

        public async Task<bool> ActualizarConfiguracionAsync(string nombreConfig, string nuevoValor)
        {
            try
            {
                var config = await ObtenerConfiguracionPorNombreAsync(nombreConfig);
                if (config == null)
                    return false;

                config.Valor = nuevoValor;
                config.FechaModificacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string ObtenerValor(List<ConfiguracionServicio> configs, string nombre, string valorPorDefecto)
        {
            var config = configs.FirstOrDefault(c => c.NombreConfig == nombre);
            return config?.Valor ?? valorPorDefecto;
        }
    }
}
