using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Authorization;
using _2024_1C_carritoDeCompras.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace _2024_1C_carritoDeCompras.Controllers
{
    public class ProductosController : Controller
    {
        private readonly CarritoDeComprasContext _context;
        private readonly UserManager<Persona> _userManager;

        public ProductosController(CarritoDeComprasContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Productos
        public IActionResult Index()
        {
            ProductModel p = new ProductModel();
            p.cat = _context.Categorias.ToList();
            p.productos = _context.Productos.ToList();

            return View(p);
        }

        // GET: Productos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefault(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // Acción para añadir productos al carrito
        public async Task<IActionResult> AddToCart(int? id) {
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null || producto.Activo == false)
            {
                return NotFound();
            }
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.ClienteId == usuarioActual.Id && c.Activo);


            if (carrito == null)
            {
                carrito = new Carrito
                {
                    Activo = true,
                    ClienteId = usuarioActual.Id,
                    Items = new List<CarritoItem>()
                };
                _context.Carritos.Add(carrito);
            }


            var carritoItem = new CarritoItem
            {
                Producto = producto,
                ValorUnitario = producto.PrecioVigente,
                Cantidad = 1,
                CarritoId = carrito.Id
            };



            carrito.Subtotal += (int)(producto.PrecioVigente);
            await _context.SaveChangesAsync();

            var carritoItemsController = new CarritoItemsController(_context, _userManager);
            await carritoItemsController.Create(carritoItem);

            return RedirectToAction(nameof(Index));
        }

        // GET: Productos/Create
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion");
            return View();
        }

        // POST: Productos/Create
        [Authorize(Roles = "Administrador,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Descripcion,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Descripcion,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productoEnDB = _context.Productos.Find(producto.Id);
                    if (productoEnDB != null)
                    {
                        productoEnDB.Nombre = producto.Nombre;
                        productoEnDB.Descripcion = producto.Descripcion;
                        productoEnDB.PrecioVigente = producto.PrecioVigente;
                        productoEnDB.Activo = producto.Activo;
                        productoEnDB.CategoriaId = producto.CategoriaId;

                        _context.Productos.Update(productoEnDB);
                        _context.SaveChanges();

                    }
                    else
                    {
                        return NotFound();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);
            return View(producto);
        }
        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
