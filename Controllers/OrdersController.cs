using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;

public class OrdersController : Controller
{
    private readonly ShoppingContext _context;

    public OrdersController(ShoppingContext context)
    {
        _context = context;
    }

    // GET: Orders
    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
        {
            // Admins see all orders
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket);
            return View(await orders.ToListAsync());
        }
        else if (User.IsInRole("Buyer"))
        {
            // Buyers see only their own orders
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return NotFound();
            }

            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .Where(o => o.CustomerId == currentUser.Id);
            return View(await orders.ToListAsync());
        }
        else
        {
            return Forbid(); // Forbidden if the user is neither Admin nor Buyer
        }
    }

    // GET: Orders/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.SuperMarket)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        if (User.IsInRole("Buyer") && order.CustomerId != GetCurrentUserId())
        {
            return Forbid(); // Buyers cannot view orders that do not belong to them
        }

        return View(order);
    }

    // GET: Orders/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID");
        ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "ID");
        return View();
    }

    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
    {
        if (ModelState.IsValid)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID", order.CustomerId);
        ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "ID", order.SuperMarketId);
        return View(order);
    }

    // GET: Orders/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID", order.CustomerId);
        ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "ID", order.SuperMarketId);
        return View(order);
    }

    // POST: Orders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
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
        ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "ID", order.CustomerId);
        ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "ID", "ID", order.SuperMarketId);
        return View(order);
    }

    // GET: Orders/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.SuperMarket)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrderExists(int id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }

    private int GetCurrentUserId()
    {
        var userEmail = User.Identity.Name;
        var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
        return user?.Id ?? -1;
    }
}
