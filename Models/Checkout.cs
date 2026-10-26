namespace AssetPortal.Models;

public class Checkout
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public Asset Asset { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    public DateTime CheckedOutAt { get; set; } = DateTime.Now;
    public DateTime? ReturnedAt { get; set; }

}