using CargaMasivaDatos.Core.Interfaces;
using CargaMasivaDatos.Core.Models;
using CargaMasivaDatos.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Core.Services
{
    public class ProcesadorPedidos : IProcesadorPedidos
    {
        private readonly IRepositorioOrigen _repoOrigen;
        private readonly IRepositorioDestino _repoDestino;
        private readonly IGeneradorJSON _generadorJSON;
        private readonly IGestorReintentos _gestorReintentos;
        private readonly IConfiguracionServicio _configuracion;
        private static int _contadorLote = 0;

        public ProcesadorPedidos(
            IRepositorioOrigen repoOrigen,
            IRepositorioDestino repoDestino,
            IGeneradorJSON generadorJSON,
            IGestorReintentos gestorReintentos,
            IConfiguracionServicio configuracion)
        {
            _repoOrigen = repoOrigen;
            _repoDestino = repoDestino;
            _generadorJSON = generadorJSON;
            _gestorReintentos = gestorReintentos;
            _configuracion = configuracion;
        }

        public bool ValidarHorarioEjecucion()
        {
            var config = _configuracion.ObtenerConfiguracion();

            if (!config.ServicioActivo)
                return false;

            var horaActual = DateTime.Now.TimeOfDay;
            return horaActual >= config.HoraInicio && horaActual <= config.HoraFin;
        }

        public async Task<ResultadoProcesamientoDTO> ProcesarPedidosAsync()
        {
            var resultado = new ResultadoProcesamientoDTO
            {
                LoteID = ++_contadorLote,
                FechaInicio = DateTime.Now
            };

            var log = new LogProcesamiento
            {
                LoteID = resultado.LoteID,
                FechaInicio = resultado.FechaInicio,
                Estado = "Procesando"
            };

            int logId = 0;

            try
            {
                // Validar horario
                if (!ValidarHorarioEjecucion())
                {
                    resultado.Exitoso = false;
                    resultado.Mensaje = "Fuera del horario de ejecución configurado";
                    return resultado;
                }

                // Registrar inicio del log
                logId = await _repoDestino.RegistrarLogProcesamientoAsync(log);

                // Obtener configuración
                var config = _configuracion.ObtenerConfiguracion();

                // Obtener pedidos pendientes
                var pedidosPendientes = await _repoOrigen.ObtenerPedidosPendientesAsync(config.TamañoLote);

                if (!pedidosPendientes.Any())
                {
                    resultado.Exitoso = true;
                    resultado.Mensaje = "No hay pedidos pendientes por procesar";
                    log.Estado = "Completado";
                    log.Mensaje = resultado.Mensaje;
                }
                else
                {
                    // Procesar cada pedido
                    foreach (var pedido in pedidosPendientes)
                    {
                        var procesado = await ProcesarPedidoIndividualAsync(pedido, resultado.LoteID);

                        if (procesado)
                        {
                            resultado.RegistrosProcesados++;
                        }
                        else
                        {
                            resultado.RegistrosError++;
                            resultado.Errores.Add($"Error procesando pedido {pedido.PedidoID}");
                        }
                    }

                    resultado.Exitoso = resultado.RegistrosError == 0;
                    resultado.Mensaje = $"Procesados: {resultado.RegistrosProcesados}, Errores: {resultado.RegistrosError}";

                    log.Estado = resultado.Exitoso ? "Completado" : "Completado con errores";
                    log.Mensaje = resultado.Mensaje;
                }

                log.RegistrosProcesados = resultado.RegistrosProcesados;
                log.RegistrosError = resultado.RegistrosError;
            }
            catch (Exception ex)
            {
                resultado.Exitoso = false;
                resultado.Mensaje = $"Error crítico: {ex.Message}";
                resultado.Errores.Add(ex.Message);

                log.Estado = "Error";
                log.Mensaje = ex.Message;
            }
            finally
            {
                // Actualizar log con los resultados finales
                resultado.FechaFin = DateTime.Now;
                log.FechaFin = resultado.FechaFin;

                if (logId > 0)
                {
                    log.LogID = logId;
                    await _repoDestino.ActualizarLogProcesamientoAsync(log);
                }
            }

            return resultado;
        }

        private async Task<bool> ProcesarPedidoIndividualAsync(Pedido pedido, int loteId)
        {
            try
            {
                // Usar el gestor de reintentos para procesar el pedido
                return await _gestorReintentos.IntentarProcesarConReintentosAsync(
                    pedido.PedidoID,
                    async () =>
                    {
                        // Obtener pedido completo con todas sus relaciones
                        var pedidoCompleto = await _repoOrigen.ObtenerPedidoCompletoAsync(pedido.PedidoID);

                        if (pedidoCompleto == null)
                            return false;

                        // Generar JSON
                        var json = _generadorJSON.GenerarJSON(pedidoCompleto, loteId);

                        // Validar JSON
                        if (!_generadorJSON.ValidarJSON(json))
                            return false;

                        // Guardar JSON en BD_Destino
                        var guardado = await _repoDestino.GuardarJSONAsync(pedidoCompleto.PedidoID, json, loteId);

                        if (!guardado)
                            return false;

                        // Marcar como procesado en BD_Origen
                        return await _repoOrigen.MarcarPedidoComoProcesadoAsync(pedidoCompleto.PedidoID);
                    });
            }
            catch
            {
                return false;
            }
        }
    }
}