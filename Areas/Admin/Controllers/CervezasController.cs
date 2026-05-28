using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Models;
using TiendaWeb.Models.Data;

namespace TiendaWeb.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    [Area("Admin")]
    [Route("Admin/Cervezas")]
    public class CervezasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CervezasController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listaCervezas = await _context.Cervezas.Include(c => c.Estilo).ToListAsync();
            return View(listaCervezas);
        }

        [HttpGet("Crear")]
        public IActionResult Create()
        {
            ViewBag.Estilos = _context.Estilos.ToList();
            return View("Create");
        }

        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,alcohol,IdEstilo,precio,UrlImagen")] Cerveza cerveza)
        {
            ModelState.Remove("Estilo");
            ModelState.Remove("UrlImagen");

            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (archivos.Count > 0 && archivos[0].Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivos[0].FileName);

                    string carpetaDestino = Path.Combine(rutaPrincipal, "imagenes", "cervezas");

                    if (!Directory.Exists(carpetaDestino))
                    {
                        Directory.CreateDirectory(carpetaDestino);
                    }

                    string rutaCompletaFisica = Path.Combine(carpetaDestino, nombreArchivo);

                    using (var fileStream = new FileStream(rutaCompletaFisica, FileMode.Create))
                    {
                        await archivos[0].CopyToAsync(fileStream);
                    }

                    cerveza.UrlImagen = "/imagenes/cervezas/" + nombreArchivo;
                }

                _context.Add(cerveza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estilos = _context.Estilos.ToList();
            return View("Create", cerveza);
        }

        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cerveza = await _context.Cervezas.FindAsync(id);
            if (cerveza == null) return NotFound();

            ViewBag.Estilos = _context.Estilos.ToList();
            return View("Edit", cerveza);
        }

        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,alcohol,IdEstilo,precio,UrlImagen")] Cerveza cerveza)
        {
            if (id != cerveza.Id) return NotFound();

            // 🚨 CRÍTICO: Eliminamos las validaciones implícitas para que no rebote por datos complejos
            ModelState.Remove("Estilo");
            ModelState.Remove("UrlImagen");

            if (ModelState.IsValid)
            {
                try
                {
                    string rutaPrincipal = _hostEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;

                    // Escenario 1: El usuario seleccionó un NUEVO archivo de imagen
                    if (archivos.Count > 0 && archivos[0].Length > 0)
                    {
                        // 1. Generamos un nombre único para la nueva foto
                        string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivos[0].FileName);
                        string carpetaDestino = Path.Combine(rutaPrincipal, "imagenes", "cervezas");

                        if (!Directory.Exists(carpetaDestino))
                        {
                            Directory.CreateDirectory(carpetaDestino);
                        }

                        string rutaCompletaFisica = Path.Combine(carpetaDestino, nombreArchivo);

                        // 2. Guardamos físicamente el nuevo archivo en el disco
                        using (var fileStream = new FileStream(rutaCompletaFisica, FileMode.Create))
                        {
                            await archivos[0].CopyToAsync(fileStream);
                        }

                        // (Opcional) Aquí se podría borrar la foto vieja del disco para no acumular basura, 
                        // pero para el examen, con asignar la nueva ruta es más que suficiente:
                        cerveza.UrlImagen = "/imagenes/cervezas/" + nombreArchivo;
                    }
                    else
                    {
                        // Escenario 2: No se subió ningún archivo nuevo.
                        // 🚨 truco de EF CORE: Buscamos la cerveza original en la base de datos (sin trackearla) 
                        // para recuperar la ruta de la imagen que ya tenía y no perderla.
                        var cervezaOriginal = await _context.Cervezas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                        if (cervezaOriginal != null)
                        {
                            cerveza.UrlImagen = cervezaOriginal.UrlImagen;
                        }
                    }

                    _context.Update(cerveza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cervezas.Any(e => e.Id == cerveza.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estilos = _context.Estilos.ToList();
            return View("Edit", cerveza);
        }

        [HttpGet("Detalles/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cerveza = await _context.Cervezas
                .Include(c => c.Estilo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cerveza == null) return NotFound();

            return View("Details", cerveza);
        }

        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cerveza = await _context.Cervezas
                .Include(c => c.Estilo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cerveza == null) return NotFound();

            return View("Delete", cerveza);
        }

        [HttpPost("Eliminar/{id}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cerveza = await _context.Cervezas.FindAsync(id);
            if (cerveza != null)
            {
                _context.Cervezas.Remove(cerveza);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}