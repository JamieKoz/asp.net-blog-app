using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace cs_blog_app.Controllers;

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
        var blogs = await _context.Blogs.ToListAsync();
        return View(blogs);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Blog blog)
    {
        if (ModelState.IsValid)
        {
            _context.Add(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(blog);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null)
        {
            return NotFound();
        }
        return View(blog);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Blog blog)
    {
        if (id != blog.BlogId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(blog);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blog = await _context.Blogs
            .FirstOrDefaultAsync(m => m.BlogId == id);
        if (blog == null)
        {
            return NotFound();
        }

        return View(blog);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog != null)
        {
            _context.Remove(blog);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
