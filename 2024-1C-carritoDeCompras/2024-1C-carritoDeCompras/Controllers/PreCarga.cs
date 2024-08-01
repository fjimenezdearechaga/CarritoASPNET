using _2024_1C_carritoDeCompras.Data;
using Microsoft.AspNetCore.Mvc;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;

namespace _2024_1C_carritoDeCompras.Controllers
{
    public class PreCarga : Controller
    {
        private readonly CarritoDeComprasContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;


        public PreCarga(CarritoDeComprasContext context, UserManager<Persona> userManager,
            RoleManager<Rol> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        private string passDefault = "Password1!";

        public IActionResult Seed() {
            CrearSucursales();
            CrearCategoriasYProductosYStockItem().Wait();
            CrearRolesBase().Wait();
            CrearEmpleado().Wait();
            CrearAdmin().Wait();
           

            return RedirectToAction("Index", "Home");
        }

 

        private List<Sucursal> sucursales = new List<Sucursal>()
        {
            new Sucursal(){Direccion="Montevideo 300",Telefono="48112342",Email="sucursalMontevideo@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Santa Fe 4010",Telefono = "48090011",Email = "sucursalSantaFe@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Rivadavia 10000",Telefono = "45012233",Email = "sucursalRivadavia@empresa.com.ar"},
            new Sucursal() { Direccion = "Hipolito Yrigoyen 2505",Telefono = "43032244",Email = "sucursalYrigoyen@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Caseros",Telefono = "44042342",Email = "sucursalCaseros@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Libertador 3004",Telefono = "48024422",Email = "sucursalLibertador@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Congreso 1010",Telefono = "43003388",Email = "sucursalCongreso@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Kramer 4444",Telefono = "43331199",Email = "sucursalKramer@empresa.com.ar"},
            new Sucursal() { Direccion = "Yatay 4422",Telefono = "44001111",Email = "sucursalYatay@empresa.com.ar"},
            new Sucursal() { Direccion = "Av Belgrano",Telefono = "45009988",Email = "sucursalBelgrano@empresa.com.ar"},
    };


        private void CrearSucursales()
        {
            foreach (Sucursal sucursal in sucursales)
            {
                _context.Sucursales.Add(sucursal);
                _context.SaveChanges();
            }
        }


        private List<Categoria> categorias = new List<Categoria>
{
    new Categoria() { Nombre = "Vehiculos", Descripcion = "Monopatines Electricos, Motos y Bicicletas" },
    new Categoria() { Nombre = "Supermercado", Descripcion = "Productos tipicos del supermercado local" },
    new Categoria() { Nombre = "Tecnologia", Descripcion = "Computacion, TV, Gaming" },
    new Categoria() { Nombre = "Arte", Descripcion = "Libros, Musica, Peliculas." },
    new Categoria() { Nombre = "Deportes", Descripcion = "Accesorios para la practica de deportes" },
    new Categoria() { Nombre = "Libreria", Descripcion = "Materiales para el colegio" },
    new Categoria() { Nombre = "Ropa", Descripcion = "Ropa de todos los tipos" },
    new Categoria() { Nombre = "Juegos", Descripcion = "Juegos de mesa y juguetes para los chicos" },
    new Categoria() { Nombre = "Salud", Descripcion = "Suplementos, Equipos medicos, etc" },
    new Categoria() { Nombre = "Construccion", Descripcion = "Material de obra, Herramientas de ferreteria,etc" }
};

        private List<Producto> productos = new List<Producto>
{
    new Producto() { Nombre = "Zenith", Descripcion = "Bicicletas con 10 cambios",PrecioVigente=180000,Activo=true },
    new Producto() { Nombre = "Off", Descripcion = "Aerosol contra los mosquitos",PrecioVigente=20000,Activo=false  },
    new Producto() { Nombre = "Mac Mini", Descripcion = "Computadora de escritorio",PrecioVigente=2444000,Activo=true  },
    new Producto() { Nombre = "Sound and Fury", Descripcion = "Novela del siglo 20" ,PrecioVigente=15000,Activo=true },
    new Producto() { Nombre = "Fitlife", Descripcion = "Colchoneta para actividad fisica",PrecioVigente=144000,Activo=false  },
    new Producto() { Nombre = "Rivadavia", Descripcion = "Cuaderno rayado",PrecioVigente=50000,Activo=true  },
    new Producto() { Nombre = "Bensimon 100", Descripcion = "Camiseta blanca",PrecioVigente=444444,Activo=false  },
    new Producto() { Nombre = "T.A.G", Descripcion = "Juego de estrategia" ,PrecioVigente=333444,Activo=true },
    new Producto() { Nombre = "Ibupirac100", Descripcion = "Ibuprofeno",PrecioVigente=18000,Activo=true  },
    new Producto() { Nombre = "SierraJohnDeer", Descripcion = "Sierra hogarenia.",PrecioVigente=1800004,Activo=false  }
};


        private List<Producto> productos2 = new List<Producto>
{
    new Producto() { Nombre = "Zenith10", Descripcion = "Bicicletas con 10 cambios",PrecioVigente=180000,Activo=true },
    new Producto() { Nombre = "Off2", Descripcion = "Aerosol contra los mosquitos",PrecioVigente=20000,Activo=false  },
    new Producto() { Nombre = "Mac Mini B", Descripcion = "Computadora de escritorio",PrecioVigente=2444000,Activo=true  },
    new Producto() { Nombre = "LaDivinaComedia", Descripcion = "Novela del siglo 15" ,PrecioVigente=15000,Activo=true },
    new Producto() { Nombre = "Joeys", Descripcion = "Colchoneta para actividad fisica",PrecioVigente=144000,Activo=false  },
    new Producto() { Nombre = "RivadaviaBlue", Descripcion = "Cuaderno rayado",PrecioVigente=50000,Activo=true  },
    new Producto() { Nombre = "Bensimon 50", Descripcion = "Camiseta blanca",PrecioVigente=444444,Activo=false  },
    new Producto() { Nombre = "WAR", Descripcion = "Juego de estrategia" ,PrecioVigente=333444,Activo=true },
    new Producto() { Nombre = "Ibupirac50", Descripcion = "Ibuprofeno",PrecioVigente=18000,Activo=true  },
    new Producto() { Nombre = "JohnDeer444", Descripcion = "Sierra hogarenia.",PrecioVigente=1800004,Activo=false }
};
        private async Task CrearCategoriasYProductosYStockItem()
        {
            int i = 0;
            foreach (Categoria categoria in categorias)
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                productos[i].CategoriaId = categoria.Id;
                productos2[i].CategoriaId = categoria.Id;
                i++;
            }

            foreach (Producto producto in productos)
            {
                _context.Productos.Add(producto);
                foreach (Sucursal sucursal in _context.Sucursales)
                {
                    StockItem stock = new StockItem();
                    stock.Producto = producto;
                    stock.ProductoId = producto.Id;
                    stock.Cantidad = 100;
                    stock.Sucursal = sucursal;
                    stock.SucursalId = sucursal.Id;
                    _context.Add(stock);
                }
            }
            await _context.SaveChangesAsync();
        }


        private async Task CrearRolesBase()
        {
            List<string> roles = new List<string>() { "Administrador", "Cliente", "Empleado", "Usuario" };

            foreach (string rol in roles)
            {
                await CrearRole(rol);
            }
        }

        private async Task CrearRole(string rolName)
        {
            if (!await _roleManager.RoleExistsAsync(rolName))
            {
                await _roleManager.CreateAsync(new Rol(rolName));
            }
        }

        public async Task<IActionResult> CrearAdmin()
        {
            Persona account = new Persona()
            {
                Nombre = "Admin",
                Apellido = "Del Futuro",
                Email = "admin@admin.com",
                UserName = "admin@admin.com"
            };

            var resuAdm = await _userManager.CreateAsync(account, passDefault);
            if (resuAdm.Succeeded)
            {
                string rolAdm = "Administrador";
                await _userManager.AddToRoleAsync(account, rolAdm);
                TempData["Mensaje"] = $"Empleado creado {account.Email} y {passDefault}";
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> CrearEmpleado()
        {
            Empleado account = new Empleado()
            {
                Nombre = "Empleado",
                Apellido = "Del Mes",
                Email = "empleado@empleado.com",
                UserName = "empleado@empleado.com"
            };

            var resuAdm = await _userManager.CreateAsync(account, passDefault);
            if (resuAdm.Succeeded)
            {
                string rolEmp = "Empleado";
                await _userManager.AddToRoleAsync(account, rolEmp);
                TempData["Mensaje"] = $"Empleado creado {account.Email} y {passDefault}";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
