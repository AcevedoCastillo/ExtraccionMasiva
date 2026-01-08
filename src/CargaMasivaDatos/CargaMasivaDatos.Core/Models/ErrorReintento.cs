using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models
{
    public class ErrorReintento
    {
        public int ErrorID { get; set; }
        public int? PedidoID { get; set; }
        public int? LoteID { get; set; }
        public string MensajeError { get; set; }
        public DateTime FechaError { get; set; }
        public int Reintentos { get; set; }
        public int MaxReintentos { get; set; }
        public bool Resuelto { get; set; }
    }
}
