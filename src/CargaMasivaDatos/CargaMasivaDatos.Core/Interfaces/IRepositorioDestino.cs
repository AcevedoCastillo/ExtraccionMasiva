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

        /// <summary>
        /// Guarda un error y su información de reintento
        /// </summary>
        Task<int> GuardarErrorAsync(ErrorReintento error);

        /// <summary>
        /// Marca un error como resuelto
        /// </summary>
        Task<bool> MarcarErrorComoResueltoAsync(int errorId);

        /// <summary>
        /// Obtiene errores no resueltos que pueden reintentarse
        /// </summary>
        Task<List<ErrorReintento>> ObtenerErroresPendientesAsync();

        /// <summary>
        /// Actualiza el contador de reintentos de un error
        /// </summary>
        Task<bool> ActualizarReintentosAsync(int errorId, int nuevoContador);
    }
}
