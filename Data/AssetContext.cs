using AssetPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetPortal.Data;

public class AssetContext : IdentityDbContext<IdentityUser>
{
    public AssetContext(DbContextOptions<AssetContext> options) : base(options) {  }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Checkout> Checkouts => Set<Checkout>();
}