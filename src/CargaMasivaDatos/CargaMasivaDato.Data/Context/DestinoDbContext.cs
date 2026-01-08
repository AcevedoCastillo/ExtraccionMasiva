using CargaMasivaDatos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDato.Data.Context
{
    public class DestinoDbContext : DbContext
    {
        public DestinoDbContext() : base("name=BD_Destino")
        {
            // Deshabilitar inicializador de base de datos
            Database.SetInitializer<DestinoDbContext>(null);
        }

        public DbSet<PedidoJSON> PedidosJSON { get; set; }
        public DbSet<LogProcesamiento> LogProcesamiento { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de PedidoJSON
            modelBuilder.Entity<PedidoJSON>()
                .HasKey(e => e.RegistroID);

            modelBuilder.Entity<PedidoJSON>()
                .Property(e => e.JSONData)
                .IsRequired();

            modelBuilder.Entity<PedidoJSON>()
                .Property(e => e.Estado)
                .HasMaxLength(20);

            // Configuración de LogProcesamiento
            modelBuilder.Entity<LogProcesamiento>()
                .HasKey(e => e.LogID);

            modelBuilder.Entity<LogProcesamiento>()
                .Property(e => e.Estado)
                .HasMaxLength(50);

            modelBuilder.Entity<LogProcesamiento>()
                .Property(e => e.Mensaje)
                .HasMaxLength(500);
        }
    }
}
