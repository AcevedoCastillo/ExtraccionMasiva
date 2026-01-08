using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models
{
    public class PedidoJSON
    {
        public int RegistroID { get; set; }
        public int PedidoID { get; set; }
        public string JSONData { get; set; }
        public DateTime FechaProcesamiento { get; set; }
        public int? LoteID { get; set; }
        public string Estado { get; set; }
    }
}
