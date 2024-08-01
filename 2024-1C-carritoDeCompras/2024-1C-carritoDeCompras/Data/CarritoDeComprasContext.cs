using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity



namespace _2024_1C_carritoDeCompras.Data
{
    public class CarritoDeComprasContext: IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public CarritoDeComprasContext(DbContextOptions options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            #region Establecer Nombres para los Identity Stores
            
            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");

            #endregion
        }


        public DbSet<Rol> Roles { get; set; }

        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<CarritoItem> CarritoItems { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Compra> Compras { get; set; }

        public DbSet<Direccion> Direcciones { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<StockItem> StockItems { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }


    }
}
