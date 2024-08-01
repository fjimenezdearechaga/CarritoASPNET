using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2024_1C_carritoDeCompras.Data;
using _2024_1C_carritoDeCompras.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace _2024_1C_carritoDeCompras.Controllers
{
    //[Authorize]
    public class ClientesController : Controller
    {
        private readonly CarritoDeComprasContext _context;

        public ClientesController(CarritoDeComprasContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [AllowAnonymous]
        //[Authorize(Roles = "Empleado , Cliente")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Telefono,FechaAlta,Id,Nombre,Apellido,Dni,Email")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                Carrito carrito = new Carrito();
                carrito.ClienteId = cliente.Id;
                _context.Add(carrito);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        //[Authorize(Roles = "Administrador, Cliente")]

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Telefono,FechaAlta,Id,Nombre,Apellido,Dni,Email")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteEnDB = _context.Clientes.Find(cliente.Id);
                    if (clienteEnDB == null)
                    {
                        return NotFound();
                    }
                    clienteEnDB.Telefono = cliente.Telefono;
                    clienteEnDB.FechaAlta = cliente.FechaAlta;
                    clienteEnDB.Nombre = cliente.Nombre;
                    clienteEnDB.Apellido = cliente.Apellido;
                    clienteEnDB.Dni = cliente.Dni;
                    Carrito carrito = new Carrito
                    {
                        Activo = true,
                        ClienteId = clienteEnDB.Id,
                        Subtotal = 0,
                    };
                    _context.Add(carrito);

                    if (!ActualizarEmail(cliente, clienteEnDB))
                    {
                        ModelState.AddModelError("Email", "El email ya esta en uso");
                        return View(cliente);
                    }

                    _context.Update(clienteEnDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5

        //[Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        //[Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }


        private bool ActualizarEmail(Cliente cltForm, Cliente cltDb) {
            bool resultado = true;

            try
            {
                if (!cltDb.NormalizedEmail.Equals(cltForm.Email.ToUpper()) ){
                    if (ExistEmail(cltForm.Email))
                    {
                        resultado = false;
                    }
                    else
                    {
                        cltDb.Email = cltForm.Email;
                        cltDb.NormalizedEmail = cltForm.Email.ToUpper();
                        cltDb.UserName = cltForm.Email;
                        cltDb.NormalizedUserName = cltForm.NormalizedEmail;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return resultado;
        }
        private bool ExistEmail(String email)
        {
            return _context.Clientes.Any(e => e.Email == email);
        }


        
    }
}
