using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models
{
    public class LogProcesamiento
    {
        public int LogID { get; set; }
        public int LoteID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int RegistrosProcesados { get; set; }
        public int RegistrosError { get; set; }
        public string Estado { get; set; }
        public string Mensaje { get; set; }
    }
}
