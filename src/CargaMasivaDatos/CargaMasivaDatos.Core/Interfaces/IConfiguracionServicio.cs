using CargaMasivaDatos.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IConfiguracionServicio
    {
        /// <summary>
        /// Obtiene la configuración completa del servicio desde App.config
        /// </summary>
        ConfiguracionDTO ObtenerConfiguracion();

        /// <summary>
        /// Valida que la configuración sea válida
        /// </summary>
        bool ValidarConfiguracion();

        /// <summary>
        /// Fuerza la recarga de la configuración desde App.config
        /// </summary>
        void RecargarConfiguracion();
    }
}
