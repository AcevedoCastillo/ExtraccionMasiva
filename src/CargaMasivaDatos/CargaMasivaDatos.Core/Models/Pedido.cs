using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public bool Procesado { get; set; }

        // Navegación
        public Cliente Cliente { get; set; }
        public List<DetallePedido> Detalles { get; set; }

        public Pedido()
        {
            Detalles = new List<DetallePedido>();
        }
    }
}
