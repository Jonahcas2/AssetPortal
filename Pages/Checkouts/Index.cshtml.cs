using AssetPortal.Data;
using AssetPortal.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Pages.Checkouts;

public class IndexModel : PageModel
{
    private readonly AssetContext _db;
    public IndexModel(AssetContext db) => _db = db;

    public List<Checkout> Open { get; set; } = new();

    public async Task OnGetAsync()
    {
        Open = await _db.Checkouts
            .Include(c => c.Asset)
            .Where(c => c.ReturnedAt == null)
            .OrderByDescending(c => c.CheckedOutAt)
            .ToListAsync();
    }
}