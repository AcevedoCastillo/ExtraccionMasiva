using CargaMasivaDatos.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasivaDato.Data.Context
{
    public class OrigenDbContext : DbContext
    {
        public OrigenDbContext() : base("name=BD_Origen")
        {
            // Deshabilitar inicializador de base de datos
            Database.SetInitializer<OrigenDbContext>(null);
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>()
                .HasKey(e => e.ClienteID);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Email)
                .HasMaxLength(150);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Telefono)
                .HasMaxLength(20);

            // Configuración de Producto
            modelBuilder.Entity<Producto>()
                .HasKey(e => e.ProductoID);

            modelBuilder.Entity<Producto>()
                .Property(e => e.NombreProducto)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Producto>()
                .Property(e => e.Categoria)
                .HasMaxLength(50);

            modelBuilder.Entity<Producto>()
                .Property(e => e.Precio)
                .HasPrecision(10, 2);

            // Configuración de Pedido
            modelBuilder.Entity<Pedido>()
                .HasKey(e => e.PedidoID);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.Estado)
                .HasMaxLength(20);

            modelBuilder.Entity<Pedido>()
                .HasRequired(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteID);

            modelBuilder.Entity<Pedido>()
                .HasMany(e => e.Detalles)
                .WithRequired()
                .HasForeignKey(d => d.PedidoID);

            // Configuración de DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasKey(e => e.DetalleID);

            modelBuilder.Entity<DetallePedido>()
                .Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(e => e.Subtotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DetallePedido>()
                .HasRequired(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.ProductoID);
        }
    }
}
