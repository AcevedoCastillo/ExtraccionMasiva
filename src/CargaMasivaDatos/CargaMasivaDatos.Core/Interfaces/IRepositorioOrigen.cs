using CargaMasivaDatos.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IRepositorioOrigen
    {
        /// <summary>
        /// Obtiene los pedidos que no han sido procesados
        /// </summary>
        Task<List<Pedido>> ObtenerPedidosPendientesAsync(int cantidad);

        /// <summary>
        /// Marca un pedido como procesado
        /// </summary>
        Task<bool> MarcarPedidoComoProcesadoAsync(int pedidoId);

        /// <summary>
        /// Obtiene un pedido completo con sus detalles, cliente y productos
        /// </summary>
        Task<Pedido> ObtenerPedidoCompletoAsync(int pedidoId);
    }
}
