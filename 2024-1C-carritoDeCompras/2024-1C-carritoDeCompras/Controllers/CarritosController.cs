using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace _2024_1C_carritoDeCompras.Controllers
{
    public class CarritosController : Controller
    {
        private readonly CarritoDeComprasContext _context;
        private readonly UserManager<Persona> _userManager;

        public CarritosController(CarritoDeComprasContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carritos
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var carritoDeComprasContext = _context.Carritos.Include(c => c.Cliente);
            return View(await carritoDeComprasContext.ToListAsync());
        }

        // GET: Carritos/Details/5
        [Authorize(Roles = "Admin, Cliente")] // Admin o Cliente
        public async Task<IActionResult> Details()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null)
            {
                return RedirectToAction("IniciarSesion", "Account");
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.ClienteId == usuarioActual.Id);

            var carritoItems = _context.CarritoItems.ToList();

            foreach (var item in carritoItems)
            {
                item.Producto = _context.Productos.Where(p => p.Id == item.ProductoId).FirstOrDefault();
            }


            ViewBag.Sucursales = _context.Sucursales.ToList();


            if (carrito == null)
            {
                return NotFound();
            }

            carrito.Subtotal = (int)(carrito.Items?.Sum(ci => ci.ValorUnitario * ci.Cantidad) ?? 0);

            return View(carrito);
        }
        // GET: Carritos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            return View();
        }

        // POST: Carritos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Activo,ClienteId,Subtotal")] Carrito carrito)
        {
            // 2) Solo puede haber un carrito activo por usuario en el sistema.
            var usuarioActual = await _userManager.GetUserAsync(User);

            var carritoExistente = _context.Carritos
                .Where(c => c.ClienteId == usuarioActual.Id && c.Activo)
                .FirstOrDefault();

            if (carritoExistente != null)
            {
                //Le muestro que no puede crear para que no quede un boton que no hace nada.
                ModelState.AddModelError(string.Empty, "Ya tienes un carrito activo.");
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
                return View(carrito);
            }

            if (ModelState.IsValid)
            {
                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        // GET: Carritos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        // POST: Carritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Activo,ClienteId,Subtotal")] Carrito updatedCarrito)
        {
            if (id != updatedCarrito.Id)
            {
                return NotFound();
            }

            var carritoExistente = await _context.Carritos.FindAsync(id);

            if (carritoExistente == null)
            {
                return NotFound();
            }

            if (!carritoExistente.Activo)
            {
                carritoExistente.Activo = true;
            }

            carritoExistente.Activo = updatedCarrito.Activo;
            carritoExistente.ClienteId = updatedCarrito.ClienteId;
            carritoExistente.Subtotal = updatedCarrito.Subtotal;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carritoExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carritoExistente.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carritoExistente.ClienteId);
            return View(carritoExistente);
        }

        // GET: Carritos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> BorrarItem(int? id)
        {
            Cliente usuarioActual = (Cliente)await _userManager.GetUserAsync(User);
            Carrito carrito = _context.Carritos.FirstOrDefault(c => usuarioActual.Id == c.ClienteId);
            CarritoItem item = _context.CarritoItems.FirstOrDefault(c => c.CarritoId == carrito.Id && c.Id == id);

            if (item != null)
            {
                _context.CarritoItems.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details");
            }
            return RedirectToAction("Details");
        }


        // GET: Carritos/Delete/5 
        private bool CarritoExists(int id)
        {
            return (_context.Carritos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int productoId, int cantidad)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("IniciarSesion", "Account");
            }

            var producto = await _context.Productos.FindAsync(productoId);

            if (producto == null)
            {
                return NotFound();
            }

            var usuarioActual = await _userManager.GetUserAsync(User);

            if (usuarioActual == null)
            {
                return RedirectToAction("IniciarSesion", "Account");
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
                Cantidad = cantidad,
                CarritoId = carrito.Id
            };

            carrito.Subtotal += (int)(producto.PrecioVigente * cantidad);

            await _context.SaveChangesAsync();

            var carritoItemsController = new CarritoItemsController(_context, _userManager);
            await carritoItemsController.Create(carritoItem);

            return RedirectToAction("Details");
        }

        public async Task<IActionResult> VaciarCarrito()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);

                var carritoActual = await _context.Carritos
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.ClienteId == usuarioActual.Id);

                if (carritoActual != null && carritoActual.Items != null)
                {
                    _context.CarritoItems.RemoveRange(carritoActual.Items);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details");
                }
            }

            var carritoVacio = new Carrito();
            return View(carritoVacio);
        }

    }
}

