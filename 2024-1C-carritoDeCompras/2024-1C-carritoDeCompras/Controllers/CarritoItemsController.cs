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
using System.Security.Claims;

namespace _2024_1C_carritoDeCompras.Controllers
{
    public class CarritoItemsController : Controller
    {
        private readonly CarritoDeComprasContext _context;
        private readonly UserManager<Persona> _userManager;


        public CarritoItemsController(CarritoDeComprasContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CarritoItems
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Index()
        {
            var carritoDeComprasContext = _context.CarritoItems.Include(c => c.Carrito);
            return View(await carritoDeComprasContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MisItems()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            var carrito =
                _context.Carritos
                .Include(c => c.Items).ThenInclude(m => m.Producto)
                .Include(c => c.Cliente)
                .FirstOrDefault(m => m.ClienteId == usuarioActual.Id && m.Activo == true);


            return View(carrito.Items);
        }

        // GET: CarritoItems/Details/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CarritoItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritoItems
                .Include(c => c.Carrito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }

        // GET: CarritoItems/Create
        [Authorize(Roles = "Cliente")]
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id");
            return View();
        }

        // POST: CarritoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create(CarritoItem carritoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carritoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId);
            return View(carritoItem);
        }

        // GET: CarritoItems/Edit/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CarritoItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritoItems.FindAsync(id);
            if (carritoItem == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId);
            return View(carritoItem);
        }

        // POST: CarritoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarritoId,ProductoId,ValorUnitario,Cantidad,SubTotal")] CarritoItem carritoItem)
        {
            if (id != carritoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carritoItem);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoItemExists(carritoItem.Id))
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
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId);
            return View(carritoItem);
        }

        // GET: CarritoItems/Delete/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CarritoItems == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritoItems
                .Include(c => c.Carrito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }


        // POST: CarritoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CarritoItems == null)
            {
                return Problem("Entity set 'CarritoDeComprasContext.CarritoItems'  is null.");
            }
            var carritoItem = await _context.CarritoItems.FindAsync(id);
            if (carritoItem != null)
            {
                _context.CarritoItems.Remove(carritoItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MisItems","CarritoItems");
        }

        private bool CarritoItemExists(int id)
        {
            return (_context.CarritoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}