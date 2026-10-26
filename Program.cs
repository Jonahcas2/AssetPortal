using AssetPortal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AssetContext>(options =>
    options.UseSqlite("Data Source=assets.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AssetContext>();
    if (!db.Assets.Any())
    {
        db.Assets.AddRange(
            new AssetPortal.Models.Asset { Name = "Dell Latitude 5440", AssetTag = "LAP-0001", Category = "Laptop" },
            new AssetPortal.Models.Asset { Name = "Dell 27\" Monitor", AssetTag = "MON-0001", Category = "Monitor" });
        db.SaveChanges();
    }
}

app.Run();
