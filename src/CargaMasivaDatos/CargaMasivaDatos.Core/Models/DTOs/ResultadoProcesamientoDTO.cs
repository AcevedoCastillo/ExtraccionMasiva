using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models.DTOs
{
    /// <summary>
    /// Resultado del procesamiento de un lote
    /// </summary>
    public class ResultadoProcesamientoDTO
    {
        public int LoteID { get; set; }
        public int RegistrosProcesados { get; set; }
        public int RegistrosError { get; set; }
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public List<string> Errores { get; set; }

        public ResultadoProcesamientoDTO()
        {
            Errores = new List<string>();
        }
    }
}
