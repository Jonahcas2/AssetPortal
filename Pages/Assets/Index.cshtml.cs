using AssetPortal.Data;
using AssetPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Pages.Assets;

public class IndexModel : PageModel
{
    private readonly AssetContext _db;
    public IndexModel(AssetContext db) => _db = db;

    public List<Asset> Assets { get; set; } = new();

    public async Task OnGetAsync()
    {
        Assets = await _db.Assets.OrderBy(a => a.Name).ToListAsync();
    }

    public async Task<IActionResult> OnPostReturnAsync(int id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset == null) return NotFound();
        if (asset.IsAvailable) return RedirectToPage();

        var open = await _db.Checkouts
            .Where(c => c.AssetId == id && c.ReturnedAt == null)
            .OrderByDescending(c => c.CheckedOutAt)
            .FirstOrDefaultAsync();
        
        if (open != null) open.ReturnedAt = DateTime.Now;

        asset.IsAvailable = true;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Nothing more to do.
        }

        return RedirectToPage();
    }
}

