using CargaMasivaDatos.Core.Models.DTOs;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IProcesadorPedidos
    {
        /// <summary>
        /// Ejecuta el procesamiento de pedidos
        /// </summary>
        Task<ResultadoProcesamientoDTO> ProcesarPedidosAsync();

        /// <summary>
        /// Valida si el servicio puede ejecutarse en el horario actual
        /// </summary>
        bool ValidarHorarioEjecucion();
    }
}
