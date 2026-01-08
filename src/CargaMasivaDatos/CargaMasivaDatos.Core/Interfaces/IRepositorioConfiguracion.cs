using CargaMasivaDatos.Core.Models;
using CargaMasivaDatos.Core.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IRepositorioConfiguracion
    {
        /// <summary>
        /// Obtiene todas las configuraciones del servicio
        /// </summary>
        Task<List<ConfiguracionServicio>> ObtenerConfiguracionesAsync();

        /// <summary>
        /// Obtiene una configuración específica por nombre
        /// </summary>
        Task<ConfiguracionServicio> ObtenerConfiguracionPorNombreAsync(string nombreConfig);

        /// <summary>
        /// Obtiene la configuración del servicio en formato DTO
        /// </summary>
        Task<ConfiguracionDTO> ObtenerConfiguracionServicioAsync();

        /// <summary>
        /// Actualiza el valor de una configuración
        /// </summary>
        Task<bool> ActualizarConfiguracionAsync(string nombreConfig, string nuevoValor);
    }
}
