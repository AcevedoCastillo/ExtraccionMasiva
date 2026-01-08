using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models
{
    public class ConfiguracionServicio
    {
        public int ConfigID { get; set; }
        public string NombreConfig { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
