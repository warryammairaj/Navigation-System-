using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Navigation_System.Data;
using Navigation_System.Models;
using System.Threading.Tasks;
using System.Linq;
namespace Navigation_System.Controllers
{
    public class NavigationItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavigationItemController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            var items = await _context.NavigationItems.Include(n => n.Parent).ToListAsync();
            return View(items);
        }
        public IActionResult Create()
        {
            ViewBag.Parents = new SelectList(_context.NavigationItems, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NavigationItem navigationItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(navigationItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Parents = new SelectList(_context.NavigationItems, "Id", "Title", navigationItem.ParentId);
            return View(navigationItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationItem = await _context.NavigationItems.FindAsync(id);
            if (navigationItem == null)
            {
                return NotFound();
            }

            ViewBag.Parents = new SelectList(_context.NavigationItems, "Id", "Title", navigationItem.ParentId);
            return View(navigationItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NavigationItem navigationItem)
        {
            if (id != navigationItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navigationItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavigationItemExists(navigationItem.Id))
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
            ViewBag.Parents = new SelectList(_context.NavigationItems, "Id", "Title", navigationItem.ParentId);
            return View(navigationItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationItem = await _context.NavigationItems
                .Include(n => n.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationItem == null)
            {
                return NotFound();
            }

            return View(navigationItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var navigationItem = await _context.NavigationItems.FindAsync(id);
            _context.NavigationItems.Remove(navigationItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NavigationItemExists(int id)
        {
            return _context.NavigationItems.Any(e => e.Id == id);
        }
    }
}
