using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Authorization;

namespace _2024_1C_carritoDeCompras.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CarritoDeComprasContext _context;

        public CategoriasController(CarritoDeComprasContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public IActionResult Index()
        {
            return View(_context.Categorias.ToList());
        }

        // GET: Categorias/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = _context.Categorias
                .FirstOrDefault(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [Authorize(Roles = "Administrador,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = _context.Categorias.Find(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoriaEnDB = _context.Categorias.Find(categoria.Id);
                    if (categoriaEnDB != null)
                    {
                        categoriaEnDB.Nombre = categoria.Nombre;
                        categoriaEnDB.Descripcion = categoria.Descripcion;
                        _context.Update(categoriaEnDB);
                        _context.SaveChanges();
                    }
                    else {
                        return NotFound();
                    }
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        /*
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
