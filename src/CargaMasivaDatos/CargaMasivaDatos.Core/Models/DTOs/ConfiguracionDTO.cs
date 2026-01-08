using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Models.DTOs
{
    /// <summary>
    /// DTO para facilitar el acceso a la configuración del servicio
    /// </summary>
    public class ConfiguracionDTO
    {
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int FrecuenciaMinutos { get; set; }
        public int TamañoLote { get; set; }
        public int MaxReintentos { get; set; }
        public int TiempoEsperaReintento { get; set; }
        public bool ServicioActivo { get; set; }
    }
}
