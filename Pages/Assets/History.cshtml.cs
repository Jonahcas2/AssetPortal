using AssetPortal.Data;
using AssetPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Pages.Assets;

public class HistoryModel : PageModel
{
    private readonly AssetContext _db;
    public HistoryModel(AssetContext db) => _db = db;

    public Asset Asset { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var asset = await _db.Assets
            .Include(a => a.Checkouts)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asset == null) return NotFound();

        return Page();
    }
}