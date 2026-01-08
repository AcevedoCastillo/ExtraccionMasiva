using CargaMasivaDatos.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CargaMasivaDatos.Data.Context
{
    public class OrigenDbContext : DbContext
    {
        public OrigenDbContext(DbContextOptions<OrigenDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ClienteID);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            // Configuración de Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.ProductoID);
                entity.Property(e => e.NombreProducto).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Categoria).HasMaxLength(50);
                entity.Property(e => e.Precio).HasColumnType("decimal(10,2)");
            });

            // Configuración de Pedido
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.PedidoID);
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Estado).HasMaxLength(20);

                entity.HasOne(e => e.Cliente)
                    .WithMany()
                    .HasForeignKey(e => e.ClienteID);

                entity.HasMany(e => e.Detalles)
                    .WithOne()
                    .HasForeignKey(d => d.PedidoID);
            });

            // Configuración de DetallePedido
            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.HasKey(e => e.DetalleID);
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Producto)
                    .WithMany()
                    .HasForeignKey(e => e.ProductoID);
            });
        }
    }
}
