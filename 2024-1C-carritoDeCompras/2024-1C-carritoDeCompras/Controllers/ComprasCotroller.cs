
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using _2024_1C_carritoDeCompras.Controllers;
using System.Security.Claims;

namespace _2023_2C_F_CarritoDeCompras.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly CarritoDeComprasContext _context;
        private readonly UserManager<Persona> _userManager;

        public ComprasController(CarritoDeComprasContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Compras
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Index()
        {
            Cliente cliente = (Cliente)await _userManager.GetUserAsync(User);
            var carritoDeComprasContext = _context.Compras.Include(c => c.Carrito).Where(c => c.ClienteId == cliente.Id).ToList();
            return View(carritoDeComprasContext);
        }

        [Authorize(Roles = "Empleado, Administrador")]
        public async Task<IActionResult> IndexEmpleado()
        {
            var carritoDeComprasContext = await _context.Compras.ToListAsync();
            return View(carritoDeComprasContext);
        }

        [HttpPost]
        [Authorize(Roles = "Empleado, Administrador")]
        public ActionResult IndexEmpleado(DateTime startDate, DateTime endDate)
        {
            var carritoDeComprasContext = _context.Compras
                .Where(x => x.FechaCompra >= startDate
                    && x.FechaCompra <= endDate).OrderByDescending(x => x.Total).ToList();
            return View(carritoDeComprasContext);
        }

        // GET: Compras/Details/5
        [Authorize(Roles = "Empleado, Administrador, Cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);

            var carrito =
                _context.Carritos
                .Include(c => c.Items).ThenInclude(m => m.Producto)
                .Include(c => c.Cliente)
                .FirstOrDefault(m => m.ClienteId == usuarioActual.Id && m.Activo == true);

            if (carrito.Items.Count == 0)
            {
                TempData["Vacio"] = true;
                return RedirectToAction(nameof(CarritoItemsController.MisItems), "CarritoItems");
            }

            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion");

            return View();
        }


        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCompra(int? sucursalId)
        {
            if (sucursalId == null)
            {
                return NotFound();
            }

            var usuarioActual = await _userManager.GetUserAsync(User);

            var carrito =
                _context.Carritos
                .Include(c => c.Items).ThenInclude(m => m.Producto)
                .Include(c => c.Cliente).ThenInclude(m => m.Compras)
                .FirstOrDefault(m => m.ClienteId == usuarioActual.Id && m.Activo == true);

            var miSucursal = _context.Sucursales
                .Include(c => c.StockItems).ThenInclude(p => p.Producto)
                .FirstOrDefault(m => m.Id == sucursalId);

            if (miSucursal == null || carrito == null)
            {
                return NotFound();
            }

            if (HayStockEnSucursal(miSucursal.Id, carrito.Items))
            {
                foreach (var item in carrito.Items)
                {
                    var stockItemSucursal = miSucursal.StockItems.FirstOrDefault(p => p.ProductoId == item.ProductoId);
                    stockItemSucursal.Cantidad -= item.Cantidad;
                }

                Compra compra = new Compra
                {
                    ClienteId = usuarioActual.Id,
                    Cliente = carrito.Cliente,
                    CarritoId = carrito.Id,
                    Carrito = carrito,
                    Total = carrito.Subtotal,
                    FechaCompra = DateTime.Now,
                };
                _context.Add(compra);

                carrito.Activo = false;

                Carrito nuevoCarrito = new Carrito
                {
                    Activo = true,
                    ClienteId = usuarioActual.Id,
                    Subtotal = 0,
                };
                _context.Add(nuevoCarrito);

                _context.SaveChanges();

                Tuple<Compra, Sucursal> modelo = new Tuple<Compra, Sucursal>(compra, miSucursal);

                return View(modelo);
            }
            else
            {
                var otrasSucursales = _context.Sucursales.Where(m => m.Id != miSucursal.Id).ToList();

                var sucursalesConStock = new List<Sucursal>();

                foreach (var sucursal in otrasSucursales)
                {
                    if (HayStockEnSucursal(sucursal.Id, carrito.Items))
                    {
                        sucursalesConStock.Add(sucursal);
                    }
                }

                if (sucursalesConStock.Count > 0)
                {
                    ViewBag.MensajeSinStock = "Lo sentimos, en " + miSucursal.Email + " no contamos con stock suficiente para completar el Pedido. " +
                         "¡Por favor, intenta en otra Sucursal!";

                    ViewData["SucursalId"] = new SelectList(sucursalesConStock, "Id", "Direccion");

                    return View(nameof(Create));
                }
            }

            TempData["SinStock"] = true;
            return RedirectToAction(nameof(CarritoItemsController.MisItems), "CarritoItems");
        }


        private bool HayStockEnSucursal(int id, List<CarritoItem> carritosItems)
        {
            var sucursal =
            _context.Sucursales
            .Include(s => s.StockItems).ThenInclude(m => m.Producto)
            .FirstOrDefault(m => m.Id == id);

            return carritosItems.All(c => sucursal.StockItems
                .Any(s => s.ProductoId == c.ProductoId && s.Cantidad >= c.Cantidad));
        }
    


        private bool CompraExists(int id)
        {
            return (_context.Compras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult AgradecerViewModel()
        {
            return View();
        }
        public IActionResult ErrorPage()
        {

            return View("ErrorPage");
        }

        [HttpGet]
        public async Task<IActionResult> ComprasRealizadas()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            var compras = _context.Compras
                .Include(c => c.Carrito).ThenInclude(c => c.Items).ThenInclude(c => c.Producto)
                .Include(c => c.Cliente).ThenInclude(c => c.Compras)
                .Where(c => c.ClienteId == usuarioActual.Id);

            return View(compras.ToList());
        }

    }
}
