using AssetPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Data;

public class AssetContext : DbContext
{
    public AssetContext(DbContextOptions<AssetContext> options) : base(options) {  }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Checkout> Checkouts => Set<Checkout>();
}