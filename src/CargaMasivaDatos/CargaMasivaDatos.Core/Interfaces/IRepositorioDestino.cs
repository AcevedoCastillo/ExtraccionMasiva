using CargaMasivaDatos.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IRepositorioDestino
    {
        /// <summary>
        /// Guarda el JSON de un pedido procesado
        /// </summary>
        Task<bool> GuardarJSONAsync(int pedidoId, string jsonData, int loteId);

        /// <summary>
        /// Registra un log de procesamiento
        /// </summary>
        Task<int> RegistrarLogProcesamientoAsync(LogProcesamiento log);

        /// <summary>
        /// Actualiza un log de procesamiento existente
        /// </summary>
        Task<bool> ActualizarLogProcesamientoAsync(LogProcesamiento log);
    }
}
