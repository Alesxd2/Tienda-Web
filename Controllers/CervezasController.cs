
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Models;
using TiendaWeb.Models.Data;

public class CervezasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CervezasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CERVEZAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Cervezas.ToListAsync());
    }

    // GET: CERVEZAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cerveza = await _context.Cervezas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cerveza == null)
        {
            return NotFound();
        }

        return View(cerveza);
    }

    // GET: CERVEZAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CERVEZAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,nombre,alcohol,IdEstilo,Estilo,precio")] Cerveza cerveza)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cerveza);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cerveza);
    }

    // GET: CERVEZAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cerveza = await _context.Cervezas.FindAsync(id);
        if (cerveza == null)
        {
            return NotFound();
        }
        return View(cerveza);
    }

    // POST: CERVEZAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,nombre,alcohol,IdEstilo,Estilo,precio")] Cerveza cerveza)
    {
        if (id != cerveza.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cerveza);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CervezaExists(cerveza.Id))
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
        return View(cerveza);
    }

    // GET: CERVEZAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cerveza = await _context.Cervezas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cerveza == null)
        {
            return NotFound();
        }

        return View(cerveza);
    }

    // POST: CERVEZAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cerveza = await _context.Cervezas.FindAsync(id);
        if (cerveza != null)
        {
            _context.Cervezas.Remove(cerveza);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CervezaExists(int? id)
    {
        return _context.Cervezas.Any(e => e.Id == id);
    }
}
