using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<BidList>? BidLists { get; set; }
    public DbSet<Trade>? Trades { get; set; }
    public DbSet<CurvePoint>? CurvePoints { get; set; }
    public DbSet<Rating>? Ratings { get; set; }
    public DbSet<RuleName>? RuleNames { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
