using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TiendaWeb.Models;
using TiendaWeb.Models.Data;

namespace TiendaWeb.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var cervezas = _context.Cervezas.Include(c => c.Estilo);
            return View(await cervezas.ToListAsync());
        }
        public async Task<IActionResult> Detalles(int? id)
        {
            var cerveza = await _context.Cervezas.Include(c => c.Estilo).FirstOrDefaultAsync(c => c.Id == id);
            return View(cerveza);
        }

        private bool id(Cerveza cerveza)
        {
            throw new NotImplementedException();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
