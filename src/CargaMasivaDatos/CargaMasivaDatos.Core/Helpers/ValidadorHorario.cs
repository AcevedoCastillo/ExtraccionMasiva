using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Helpers
{
    public static class ValidadorHorario
    {
        /// <summary>
        /// Valida si la hora actual está dentro del rango especificado
        /// </summary>
        public static bool EstaEnRangoHorario(TimeSpan horaActual, TimeSpan horaInicio, TimeSpan horaFin)
        {
            return horaActual >= horaInicio && horaActual <= horaFin;
        }

        /// <summary>
        /// Calcula el tiempo restante hasta la próxima ejecución
        /// </summary>
        public static TimeSpan TiempoHastaProximaEjecucion(TimeSpan horaActual, TimeSpan horaInicio)
        {
            if (horaActual < horaInicio)
            {
                return horaInicio - horaActual;
            }
            else
            {
                // Si ya pasó la hora de inicio hoy, calcular para mañana
                var mañana = TimeSpan.FromDays(1);
                return (mañana - horaActual) + horaInicio;
            }
        }

        /// <summary>
        /// Valida que el formato de hora sea correcto (HH:mm)
        /// </summary>
        public static bool ValidarFormatoHora(string hora)
        {
            return TimeSpan.TryParseExact(hora, @"hh\:mm", null, out _);
        }
    }
}