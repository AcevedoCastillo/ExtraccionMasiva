using CargaMasivaDatos.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Services
{
    public class GestorReintentos : IGestorReintentos
    {
        private readonly IConfiguracionServicio _configuracion;

        public GestorReintentos(IConfiguracionServicio configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<bool> IntentarProcesarConReintentosAsync(int pedidoId, Func<Task<bool>> accion)
        {
            var config = _configuracion.ObtenerConfiguracion();
            int intentos = 0;

            while (intentos < config.MaxReintentos)
            {
                try
                {
                    var resultado = await accion();
                    if (resultado)
                        return true;

                    intentos++;
                    if (intentos < config.MaxReintentos)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(config.TiempoEsperaReintento));
                    }
                }
                catch (Exception)
                {
                    intentos++;
                    if (intentos < config.MaxReintentos)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(config.TiempoEsperaReintento));
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return false;
        }
    }
}
