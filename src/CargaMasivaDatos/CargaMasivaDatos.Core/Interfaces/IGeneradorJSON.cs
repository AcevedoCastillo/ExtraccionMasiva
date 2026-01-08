using CargaMasivaDatos.Core.Models;

namespace CargaMasivaDatos.Core.Interfaces
{
    public interface IGeneradorJSON
    {
        /// <summary>
        /// Genera un JSON a partir de un pedido
        /// </summary>
        string GenerarJSON(Pedido pedido, int loteId);

        /// <summary>
        /// Valida que el JSON generado sea válido
        /// </summary>
        bool ValidarJSON(string json);
    }
}
