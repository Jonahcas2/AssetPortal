using AssetPortal.Data;
using AssetPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetPortal.Pages.Assets;

public class CreateModel : PageModel
{
    private readonly AssetContext _db;
    public CreateModel(AssetContext db) => _db = db;

    [BindProperty]
    public Asset Asset { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        
        _db.Assets.Add(Asset);
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}