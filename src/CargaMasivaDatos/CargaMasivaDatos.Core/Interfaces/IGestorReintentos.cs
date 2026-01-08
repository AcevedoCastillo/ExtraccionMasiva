using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IGestorReintentos
    {
        /// <summary>
        /// Registra un error para gestionar reintentos
        /// </summary>
        Task<int> RegistrarErrorAsync(int? pedidoId, int? loteId, string mensajeError);

        /// <summary>
        /// Intenta procesar errores pendientes
        /// </summary>
        Task<int> ProcesarReintentosAsync();

        /// <summary>
        /// Verifica si un pedido puede reintentarse
        /// </summary>
        Task<bool> PuedeReintentarseAsync(int errorId);

        /// <summary>
        /// Marca un error como resuelto
        /// </summary>
        Task<bool> ResolverErrorAsync(int errorId);
    }
}
