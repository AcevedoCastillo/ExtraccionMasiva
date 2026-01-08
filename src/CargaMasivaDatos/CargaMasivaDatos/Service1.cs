using CargaMasivaDatos.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;

namespace CargaMasivaDatos
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private IServiceProvider _serviceProvider;
        private IConfiguracionServicio _configuracionServicio;
        private bool _procesandoActualmente = false;

        public Service1()
        {
            InitializeComponent();

            // Configurar nombre del servicio
            this.ServiceName = "ServicioProcesamiento";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                EscribirLog("Iniciando Servicio de Procesamiento...");

                // Configurar la inyección de dependencias
                ConfigurarDependencias();

                // Obtener configuración
                _configuracionServicio = _serviceProvider.GetService<IConfiguracionServicio>();

                // Validar configuración
                if (!_configuracionServicio.ValidarConfiguracion())
                {
                    EscribirLog("ERROR: Configuración inválida en App.config", true);
                    throw new InvalidOperationException("Configuración inválida");
                }

                var config = _configuracionServicio.ObtenerConfiguracion();
                EscribirLog($"Configuración cargada - Frecuencia: {config.FrecuenciaMinutos} minutos");
                EscribirLog($"Horario: {config.HoraInicio} - {config.HoraFin}");
                EscribirLog($"Tamaño de lote: {config.TamañoLote}");

                // Configurar el timer
                _timer = new Timer();
                _timer.Interval = TimeSpan.FromMinutes(config.FrecuenciaMinutos).TotalMilliseconds;
                _timer.Elapsed += Timer_Elapsed;
                _timer.AutoReset = true;
                _timer.Enabled = true;

                EscribirLog("Servicio iniciado correctamente");

                // Ejecutar inmediatamente la primera vez (opcional)
                // Timer_Elapsed(null, null);
            }
            catch (Exception ex)
            {
                EscribirLog($"ERROR al iniciar el servicio: {ex.Message}", true);
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                EscribirLog("Deteniendo Servicio de Procesamiento...");

                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Dispose();
                }

                // Esperar si hay un procesamiento en curso
                int intentos = 0;
                while (_procesandoActualmente && intentos < 30)
                {
                    System.Threading.Thread.Sleep(1000);
                    intentos++;
                }

                EscribirLog("Servicio detenido correctamente");
            }
            catch (Exception ex)
            {
                EscribirLog($"ERROR al detener el servicio: {ex.Message}", true);
            }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Evitar ejecuciones concurrentes
            if (_procesandoActualmente)
            {
                EscribirLog("Procesamiento anterior aún en curso. Saltando esta ejecución.");
                return;
            }

            try
            {
                _procesandoActualmente = true;
                EscribirLog("=== Iniciando ciclo de procesamiento ===");

                // Crear un scope para servicios con ciclo de vida Scoped
                using (var scope = _serviceProvider.CreateScope())
                {
                    var procesador = scope.ServiceProvider.GetService<IProcesadorPedidos>();

                    // Validar horario
                    if (!procesador.ValidarHorarioEjecucion())
                    {
                        EscribirLog("Fuera del horario de ejecución configurado");
                        return;
                    }

                    // Procesar pedidos
                    var resultado = procesador.ProcesarPedidosAsync().GetAwaiter().GetResult();

                    // Registrar resultado
                    EscribirLog($"Lote {resultado.LoteID} - Procesados: {resultado.RegistrosProcesados}, " +
                               $"Errores: {resultado.RegistrosError}");

                    if (!resultado.Exitoso)
                    {
                        EscribirLog($"ADVERTENCIA: {resultado.Mensaje}", true);

                        if (resultado.Errores.Any())
                        {
                            foreach (var error in resultado.Errores)
                            {
                                EscribirLog($"  - {error}", true);
                            }
                        }
                    }
                    else
                    {
                        EscribirLog(resultado.Mensaje);
                    }
                }

                EscribirLog("=== Ciclo de procesamiento finalizado ===");
            }
            catch (Exception ex)
            {
                EscribirLog($"ERROR en el procesamiento: {ex.Message}", true);
                EscribirLog($"StackTrace: {ex.StackTrace}", true);
            }
            finally
            {
                _procesandoActualmente = false;
            }
        }

        private void ConfigurarDependencias()
        {
            var services = new ServiceCollection();

            // Registrar DbContexts (Entity Framework 6 los crea con su connection string del App.config)
            services.AddScoped<CargaMasivaDato.Data.Context.OrigenDbContext>();
            services.AddScoped<CargaMasivaDato.Data.Context.DestinoDbContext>();

            // Registrar ConfiguracionServicio como Singleton
            services.AddSingleton<CargaMasivaDatos.Core.Interfaces.IConfiguracionServicio,
                Core.Services.ConfiguracionServicio>();

            // Registrar Repositorios como Scoped
            services.AddScoped<CargaMasivaDatos.Core.Interfaces.IRepositorioOrigen,
                CargaMasivaDato.Data.Repositories.RepositorioOrigen>();
            services.AddScoped<CargaMasivaDatos.Core.Interfaces.IRepositorioDestino,
                CargaMasivaDato.Data.Repositories.RepositorioDestino>();

            // Registrar Services como Scoped
            services.AddScoped<CargaMasivaDatos.Core.Interfaces.IGeneradorJSON,
                CargaMasivaDatos.Core.Services.GeneradorJSON>();
            services.AddScoped<CargaMasivaDatos.Core.Interfaces.IGestorReintentos,
                CargaMasivaDatos.Core.Services.GestorReintentos>();
            services.AddScoped<CargaMasivaDatos.Core.Interfaces.IProcesadorPedidos,
                CargaMasivaDatos.Core.Services.ProcesadorPedidos>();

            // Construir el ServiceProvider
            _serviceProvider = services.BuildServiceProvider();
        }

        private void EscribirLog(string mensaje, bool esError = false)
        {
            try
            {
                string tipo = esError ? "ERROR" : "INFO";
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{tipo}] {mensaje}";

                // Escribir en Event Viewer de Windows
                if (!EventLog.SourceExists(this.ServiceName))
                {
                    EventLog.CreateEventSource(this.ServiceName, "Application");
                }

                EventLogEntryType entryType = esError ? EventLogEntryType.Error : EventLogEntryType.Information;
                EventLog.WriteEntry(this.ServiceName, logMessage, entryType);

                // Opcional: También escribir a archivo
                EscribirLogArchivo(logMessage);
            }
            catch (Exception ex)
            {
                // Si falla el log, al menos intentar escribir a archivo
                EscribirLogArchivo($"[ERROR LOG] {ex.Message}");
            }
        }

        private void EscribirLogArchivo(string mensaje)
        {
            try
            {
                var logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"]
                    ?? @"C:\Logs\ServicioProcesamiento\";

                if (!System.IO.Directory.Exists(logPath))
                {
                    System.IO.Directory.CreateDirectory(logPath);
                }

                string fileName = $"ServicioLog_{DateTime.Now:yyyyMMdd}.txt";
                string fullPath = System.IO.Path.Combine(logPath, fileName);

                System.IO.File.AppendAllText(fullPath, mensaje + Environment.NewLine);
            }
            catch
            {
                // Silenciar errores de escritura de log para no afectar el servicio
            }
        }

    }
}
