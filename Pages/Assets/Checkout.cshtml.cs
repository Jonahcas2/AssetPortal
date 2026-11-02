using System.Security.Claims;
using AssetPortal.Data;
using AssetPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Pages.Assets;

public class CheckoutModel : PageModel
{
    private readonly AssetContext _db;
    public CheckoutModel(AssetContext db) => _db = db;

    public Asset Asset { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset == null) return NotFound();
        if (!asset.IsAvailable) return RedirectToPage("Index");

        Asset = asset;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var asset = await _db.Assets.FindAsync(id);
        if (asset == null) return NotFound();
        Asset = asset;

        if (!asset.IsAvailable)
        {
            ModelState.AddModelError("", "Sorry, this item was just checked out by someone else.");
            return Page();
        }

        asset.IsAvailable = false;
        _db.Checkouts.Add(new Checkout
        {
            AssetId = asset.Id,
            EmployeeName = User.Identity?.Name ?? "Unknown",
            CheckedOutByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        });

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("", "Sorry, this item was just checked out by someone else.");
            return Page();
        }

        return RedirectToPage("Index");
    }
}