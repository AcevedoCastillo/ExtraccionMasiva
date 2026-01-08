using System;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IGestorReintentos
    {
        /// <summary>
        /// Intenta procesar un pedido con reintentos
        /// </summary>
        Task<bool> IntentarProcesarConReintentosAsync(int pedidoId, Func<Task<bool>> accion);
    }
}
