using System.ComponentModel.DataAnnotations;
using AssetPortal.Models;
using AssetPortal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Pages.Assets
{
    public class CheckoutModel : PageModel
    {
        private readonly AssetContext _db;
        public CheckoutModel(AssetContext db) => _db = db;

        public Asset Asset { get; set; } = null!;

        [BindProperty, Required, StringLength(100)]
        [Display(Name = "Your Name")]
        public string EmployeeName { get; set; } = "";

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

            if (!ModelState.IsValid) return Page();

            if (!asset.IsAvailable)
            {
                ModelState.AddModelError(string.Empty, "Sorry, this asset is no longer available.");
                return Page();
            }

            asset.IsAvailable = false;
            _db.Checkouts.Add(new Checkout 
            { 
                AssetId = asset.Id, 
                EmployeeName = EmployeeName.Trim() 
            });

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Sorry, this asset was just checked out by someone else.");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
