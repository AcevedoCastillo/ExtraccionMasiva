using CargaMasivaDatos.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDatos.Data.Context
{
    public class DestinoDbContext : DbContext
    {
        public DestinoDbContext(DbContextOptions<DestinoDbContext> options)
            : base(options)
        {
        }

        public DbSet<PedidoJSON> PedidosJSON { get; set; }
        public DbSet<LogProcesamiento> LogProcesamiento { get; set; }
        public DbSet<ConfiguracionServicio> ConfiguracionServicio { get; set; }
        public DbSet<ErrorReintento> ErroresReintentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de PedidoJSON
            modelBuilder.Entity<PedidoJSON>(entity =>
            {
                entity.HasKey(e => e.RegistroID);
                entity.Property(e => e.JSONData).IsRequired();
                entity.Property(e => e.Estado).HasMaxLength(20);
            });

            // Configuración de LogProcesamiento
            modelBuilder.Entity<LogProcesamiento>(entity =>
            {
                entity.HasKey(e => e.LogID);
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.Mensaje).HasMaxLength(500);
            });

            // Configuración de ConfiguracionServicio
            modelBuilder.Entity<ConfiguracionServicio>(entity =>
            {
                entity.HasKey(e => e.ConfigID);
                entity.Property(e => e.NombreConfig).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.NombreConfig).IsUnique();
                entity.Property(e => e.Valor).HasMaxLength(500);
                entity.Property(e => e.Descripcion).HasMaxLength(250);
            });

            // Configuración de ErrorReintento
            modelBuilder.Entity<ErrorReintento>(entity =>
            {
                entity.HasKey(e => e.ErrorID);
            });
        }
    }
}
