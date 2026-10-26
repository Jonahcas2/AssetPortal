using AssetPortal.Data;
using AssetPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetPortal.Pages.Assets;

public class EditModel : PageModel
{
    private readonly AssetContext _db;
    public EditModel(AssetContext db) => _db = db;

    [BindProperty]
    public Asset Asset { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset == null) return NotFound();

        Asset = asset;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        
        var existing = await _db.Assets.FindAsync(Asset.Id);
        if (existing == null) return NotFound();

        existing.Name = Asset.Name;
        existing.AssetTag = Asset.AssetTag;
        existing.Category = Asset.Category;

        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}