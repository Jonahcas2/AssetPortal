using System.Security.Claims;
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

    public IList<Asset> Assets { get; set; } = null!;

    public async Task OnGetAsync()
    {
        Assets = await _db.Assets
            .Include(a => a.Checkouts.Where(c => c.ReturnedAt == null))
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostReturnAsync(int id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset == null) return NotFound();
        if (asset.IsAvailable) return RedirectToPage(); // Nothing to return

        var open = await _db.Checkouts
            .Where(c => c.AssetId == id && c.ReturnedAt == null)
            .OrderByDescending(c => c.CheckedOutAt)
            .FirstOrDefaultAsync();
        
        // Server-side permission check: admins, or the person who checked it out
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isOwner = open != null && open.CheckedOutByUserId == userId;
        if (!User.IsInRole("Admin") && !isOwner) return Forbid();

        if (open != null) open.ReturnedAt = DateTime.UtcNow;

        asset.IsAvailable = true;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Someone else already returned it, so just ignore and redirect
        }

        return RedirectToPage();
        
    }
}