using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace _2024_1C_carritoDeCompras.Controllers
{
    public class SucursalsController : Controller
    {
        private readonly CarritoDeComprasContext _context;

        public SucursalsController(CarritoDeComprasContext context)
        {
            _context = context;
        }

        // GET: Sucursals
        public IActionResult Index()
        {
            return View(_context.Sucursales.ToList());
        }

        // GET: Sucursals/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = _context.Sucursales
                .FirstOrDefault(m => m.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // GET: Sucursals/Create
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sucursals/Create
        [Authorize(Roles = "Administrador,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                _context.Sucursales.Add(sucursal);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(sucursal);
        }

        // GET: Sucursals/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal =  _context.Sucursales.Find(id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursals/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (id != sucursal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sucursalEnDB = _context.Sucursales.Find(sucursal.Id);
                    if (sucursalEnDB != null)
                    {
                        sucursalEnDB.Direccion = sucursal.Direccion;
                        sucursalEnDB.Telefono = sucursal.Telefono;
                        sucursalEnDB.Email = sucursal.Email;
                        _context.Sucursales.Update(sucursalEnDB);
                        _context.SaveChanges();
                    }
                    else {
                        return NotFound();
                 
                    }

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SucursalExists(sucursal.Id))
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
            return View(sucursal);
        }

        [Authorize(Roles="Administrador,Empleado")]
        // GET: Sucursals/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sucursal = _context.Sucursales
                .Include(s => s.StockItems)
                .FirstOrDefault(m => m.Id == id);

            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }


        [Authorize(Roles = "Administrador,Empleado")]
        // POST: Sucursals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var sucursal = _context.Sucursales
                 .Include(s => s.StockItems)
                 .FirstOrDefault(m => m.Id == id);

            var stock = sucursal.StockItems.Sum(s => s.Cantidad);

            if (stock == 0)
            {
                _context.Sucursales.Remove(sucursal);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else {
                ViewBag.MensajeError = "No se puede eliminar Sucursal, total productos en Stock : ";
                ViewBag.stock = stock;
            }
            return View(sucursal);

        }

        private bool HayStock(Sucursal sucursal)
        {
            int i = 0;
            bool hayStock = false;
            if (sucursal.StockItems == null)
            {
                return false;
            }
            else {
                while (i < sucursal.StockItems.Count() && !hayStock)
                {
                    if (sucursal.StockItems[i].Cantidad > 0)
                    {
                        hayStock = true;
                    }
                    else { i++; }
                }
            }
            
            return hayStock;
        }

        private bool SucursalExists(int id)
        {
            return _context.Sucursales.Any(e => e.Id == id);
        }
    }
}
