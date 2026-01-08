using CargaMasivaDatos.Core.Interfaces;
using CargaMasivaDatos.Core.Models;
using CargaMasivaDatos.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Services
{
    public class ConfiguracionServicio : IConfiguracionServicio
    {
        private ConfiguracionDTO _configuracionCache;
        public ConfiguracionDTO ObtenerConfiguracion()
        {
            // Si ya está en cache, retornarla
            if (_configuracionCache != null)
                return _configuracionCache;

            // Leer desde App.config
            _configuracionCache = new ConfiguracionDTO
            {
                HoraInicio = TimeSpan.Parse(ObtenerValor("HoraInicio", "08:00")),
                HoraFin = TimeSpan.Parse(ObtenerValor("HoraFin", "18:00")),
                FrecuenciaMinutos = int.Parse(ObtenerValor("FrecuenciaMinutos", "30")),
                TamañoLote = int.Parse(ObtenerValor("TamañoLote", "100")),
                MaxReintentos = int.Parse(ObtenerValor("MaxReintentos", "3")),
                TiempoEsperaReintento = int.Parse(ObtenerValor("TiempoEsperaReintentoSegundos", "5")),
                ServicioActivo = bool.Parse(ObtenerValor("ServicioActivo", "true"))
            };

            return _configuracionCache;
        }

        public bool ValidarConfiguracion()
        {
            try
            {
                var config = ObtenerConfiguracion();

                // Validaciones
                if (config.HoraInicio >= config.HoraFin)
                    return false;

                if (config.FrecuenciaMinutos <= 0)
                    return false;

                if (config.TamañoLote <= 0)
                    return false;

                if (config.MaxReintentos < 0)
                    return false;

                if (config.TiempoEsperaReintento < 0)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Fuerza la recarga de la configuración desde App.config
        /// </summary>
        public void RecargarConfiguracion()
        {
            _configuracionCache = null;
            ConfigurationManager.RefreshSection("appSettings");
        }

        private string ObtenerValor(string key, string valorPorDefecto)
        {
            var valor = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(valor) ? valorPorDefecto : valor;
        }
    }
}
