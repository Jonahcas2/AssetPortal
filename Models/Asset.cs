using System.ComponentModel.DataAnnotations;

namespace AssetPortal.Models;

public class Asset
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";
    
    [Required, StringLength(20)]
    [Display(Name = "Asset Tag")]
    public string AssetTag { get; set; } = "";
    
    [Required, StringLength(50)]
    public string Category { get; set; } = "";
    
    [ConcurrencyCheck]
    public bool IsAvailable { get; set; } = true;

    public List<Checkout> Checkouts { get; set; } = new();
}